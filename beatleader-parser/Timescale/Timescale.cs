using System.Collections.Generic;
using System;
using Parser.Map.Difficulty.V3.Event;
using System.Linq;
using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V3.Grid;

namespace beatleader_parser.Timescale
{
    public class Timescale
    {
        // Based on https://github.com/KivalEvan/BeatSaber-MapCheck/blob/main/src/ts/beatmap/shared/bpm.ts
        public Timescale BPM { get; set; }
        private float _bpm { get; set; }
        private List<IBPMChange> _bpmChange { get; set; }
        private List<IBPMTimeScale> _timeScale { get; set; }
        private float _offset { get; set; }
        private List<BpmRegion> _regions { get; set; }

        public Timescale(float bpm, List<IBPMChange> bpmChange, float offset)
        {
            BPM = this;
            _bpm = bpm;
            _offset = offset;
            _timeScale = GetTimeScale(bpmChange);
            _bpmChange = GetBpmChangeTime(bpmChange);
            _regions = BuildRegions(bpm, bpmChange);
        }

        private struct BpmRegion
        {
            public float startTime;
            public float startBeat;
            public float bpm;
        }

        private static List<BpmRegion> BuildRegions(float startBpm, List<IBPMChange> bpmChange)
        {
            var events = (bpmChange ?? new List<IBPMChange>()).OrderBy(e => e.b).ToList();
            var regions = new List<BpmRegion>();
            if (events.Any(e => e.startTime > 0f))
            {
                foreach (var e in events)
                    regions.Add(new BpmRegion { startTime = e.startTime, startBeat = e.b, bpm = e.m });
                return regions;
            }
            int first = 0;
            if (events.Count > 0 && events[0].b == 0f)
            {
                if (events[0].m > 0f) startBpm = events[0].m;
                first = 1;
            }
            regions.Add(new BpmRegion { startTime = 0f, startBeat = 0f, bpm = startBpm });
            for (int i = first; i < events.Count; i++)
            {
                var prev = regions[regions.Count - 1];
                regions.Add(new BpmRegion { startTime = RegionTime(prev, events[i].b), startBeat = events[i].b, bpm = events[i].m });
            }
            return regions;
        }

        private static float RegionTime(BpmRegion r, float beat)
        {
#if IL2CPP_FLOAT
            return r.startTime + (beat - r.startBeat) / r.bpm * 60f;
#else
            return (float)((double)r.startTime + ((double)beat - r.startBeat) / r.bpm * 60.0);
#endif
        }

        public static Timescale Create(float bpm, List<BpmEvent> bpmChange, float offset)
        {
            List<IBPMChange> change = new();
            foreach (var bpmEvent in bpmChange)
            {
                change.Add(new(bpmEvent));
            }
            return new Timescale(bpm, change, offset);
        }

        public float GetValue()
        {
            return _bpm;
        }

        public void SetValue(float bpm)
        {
            _bpm = bpm;
        }

        public List<IBPMChange> GetChange()
        {
            return _bpmChange;
        }

        public void SetChange(List<IBPMChange> bpmChange)
        {
            _bpmChange = bpmChange;
        }

        public List<IBPMTimeScale> GetTimescale()
        {
            return _timeScale;
        }

        public void SetTimescale(List<IBPMTimeScale> timescale)
        {
            _timeScale = timescale;
        }

        public float GetOffset()
        {
            return _offset * 1000;
        }

        public void SetOffset(float offset)
        {
            _offset = offset / 1000;
        }

        public List<IBPMChange> GetBpmChangeTime(List<IBPMChange> bpmc)
        {
            IBPMChange temp = null;
            List<IBPMChange> bpmChange = new();
            bpmc.OrderBy(b => b.b);
            for (int i = 0; i < bpmc.Count; i++)
            {
                var curBPMC = bpmc[i];
                if (temp != null)
                {
                    curBPMC.newTime = (float)Math.Ceiling(((curBPMC.b - temp.b) / _bpm) * temp.m + temp.newTime - 0.01);
                }
                else
                {
                    curBPMC.newTime = (float)Math.Ceiling(curBPMC.b - (_offset * _bpm) / 60 - 0.01);
                }
                bpmChange.Add(curBPMC);
                temp = curBPMC;
            }

            return bpmChange;
        }

        public List<IBPMTimeScale> GetTimeScale(List<IBPMTimeScale> bpmc)
        {
            bpmc = bpmc.OrderBy(b => b.time).ToList();
            List<IBPMTimeScale> timeScale = new();
            IBPMTimeScale ibpm = new();
            foreach (var bpm in bpmc)
            {
                ibpm = new()
                {
                    time = bpm.time,
                    scale = bpm.scale
                };
                timeScale.Add(ibpm);
            }

            return timeScale;
        }

        public List<IBPMTimeScale> GetTimeScale(List<IBPMChange> bpmc)
        {
            bpmc = bpmc.OrderBy(b => b.b).ToList();
            List<IBPMTimeScale> timeScale = new();
            IBPMTimeScale ibpm = new();
            foreach (var bpm in bpmc)
            {
                ibpm = new()
                {
                    time = bpm.b,
                    scale = _bpm / bpm.m
                };
                timeScale.Add(ibpm);
            }

            return timeScale;
        }

        public float OffsetBegone(float beat)
        {
            return ToBeatTime(ToRealTime(beat, false) - _offset);
        }

        public float ToRealTime(float beat, bool timescale = true)
        {
            if (!timescale || _regions == null || _regions.Count == 0 || _regions.Any(r => !(r.bpm > 0f)))
            {
#if IL2CPP_FLOAT
                return beat / _bpm * 60f;
#else
                return (float)((double)beat / _bpm * 60.0);
#endif
            }
            int idx = 0;
            while (idx < _regions.Count - 1 && _regions[idx + 1].startBeat < beat) idx++;
            return RegionTime(_regions[idx], beat);
        }

        public float ToBeatTime(float seconds, bool timescale = false)
        {
            if (!timescale)
            {
                return (seconds * _bpm) / 60;
            }
            float calculatedSecond = 0;
            for (int i = _timeScale.Count - 1; i >= 0; i--)
            {
                var currentSeconds = ToRealTime(_timeScale[i].time);
                if (seconds > currentSeconds)
                {
                    calculatedSecond += (seconds - currentSeconds) / _timeScale[i].scale;
                    seconds = currentSeconds;
                }
            }
            return ToBeatTime(seconds + calculatedSecond);
        }

        public float ToJsonTime(float beat)
        {
            for (int i = _bpmChange.Count - 1; i >= 0; i--)
            {
                if (beat > _bpmChange[i].newTime)
                {
                    return (((beat - _bpmChange[i].newTime) / _bpmChange[i].m) * _bpm + _bpmChange[i].b);
                }
            }
            return ToBeatTime(ToRealTime(beat, false) + _offset);
        }

        public float AdjustTime(float beat)
        {
            for (int i = _bpmChange.Count - 1; i >= 0; i--)
            {
                if (beat > _bpmChange[i].b)
                {
                    return (((beat - _bpmChange[i].b) / _bpm) * _bpmChange[i].m + _bpmChange[i].newTime);
                }
            }
            return OffsetBegone(beat);
        }

        public void SetCurrentBPM(float beat)
        {
            for (int i = 0; i < _bpmChange.Count; i++)
            {
                if (beat > _bpmChange[i].b)
                {
                    _bpm = _bpmChange[i].m;
                }
            }
        }

        public void ResetCurrentBPM()
        {
            if (_bpmChange.Count > 0)
            {
                _bpm = _bpmChange[0].m;
                return;
            }
        }

        public void ConvertAllBeatsToSeconds(List<BeatmapObject> objects)
        {
            foreach (var obj in objects)
            {
                obj.Seconds = BPM.ToRealTime(obj.Beats);
                obj.BpmTime = BPM.ToBeatTime(obj.Seconds);
            }
        }

        public void ConvertAllBeatsToSeconds(List<Chain> chains)
        {
            foreach (var obj in chains)
            {
                obj.TailInSeconds = BPM.ToRealTime(obj.TailInBeats);
                obj.TailBpmTime = BPM.ToBeatTime(obj.TailInSeconds);
            }
        }

        public void ConvertAllBeatsToSeconds(List<Arc> arcs)
        {
            foreach (var obj in arcs)
            {
                obj.TailInSeconds = BPM.ToRealTime(obj.TailInBeats);
                obj.TailBpmTime = BPM.ToBeatTime(obj.TailInSeconds);
            }
        }

        public void ConvertAllBeatsToSeconds(List<Wall> walls)
        {
            foreach (var obj in walls)
            {
                float end = obj.Beats + obj.DurationInBeats;
                obj.DurationInSeconds = BPM.ToRealTime(end) - BPM.ToRealTime(obj.Beats);
            }
        }

        public class IBPMTimeScale
        {
            public float time { get; set; }
            public float scale { get; set; }
        }

        public class IBPMChange
        {
            public IBPMChange(BpmEvent ev)
            {
                b = ev.Beats;
                m = ev.Bpm;
                newTime = ev.Beats;
                startTime = ev.BpmChangeStartTime;
            }
            public float startTime { get; set; } = 0;
            public float b { get; set; }
            public float m { get; set; }
            public float p { get; set; } = 0;
            public float o { get; set; } = 0;
            public float newTime { get; set; }
        }
    }
}
