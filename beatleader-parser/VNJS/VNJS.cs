using Parser.Map.Difficulty.V3.Base;
using Parser.Map.Difficulty.V3.Event;
using System.Collections.Generic;
using System.Linq;

namespace beatleader_parser.VNJS
{
    public class VNJS
    {
        public float baseNjs { get; set; }
        public List<NjsEvent> njsEvents { get; set; }

        public VNJS(float baseNjs, List<NjsEvent> njsEvents)
        {
            this.baseNjs = baseNjs;
            this.njsEvents = njsEvents.OrderBy(x => x.Seconds).ToList();
        }

        // The start of the map acts as an event with no delta, the NJS never goes below 0.01
        public float NjsAtSeconds(float seconds)
        {
            NjsEvent lastEvent = njsEvents.LastOrDefault(x => x.Seconds <= seconds);
            NjsEvent nextEvent = njsEvents.FirstOrDefault(x => x.Seconds > seconds);

            float lastSeconds = lastEvent?.Seconds ?? 0, lastDelta = lastEvent?.Delta ?? 0;
            if (nextEvent is null || nextEvent.Easing == -1 || nextEvent.Seconds <= lastSeconds) return Clamp(baseNjs + lastDelta);

            var t = (seconds - lastSeconds) / (nextEvent.Seconds - lastSeconds);
            t = t < 0 ? 0 : t > 1 ? 1 : t;
            t = (float)BeatSaberEasings.Ease(t, (BeatSaberEasingType)nextEvent.Easing);
            var njsOffset = lastDelta + t * (nextEvent.Delta - lastDelta);
            return Clamp(baseNjs + njsOffset);
        }

        private static float Clamp(float njs) => njs < 0.01f ? 0.01f : njs;

        public void CalculateAllObjectNjs(List<BeatmapGridObject> objects)
        {
            foreach (var obj in objects)
            {
                obj.njs = NjsAtSeconds(obj.Seconds);
            }
        }
    }
}
