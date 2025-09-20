using BeatMapParser.Map.Difficulty.V3.Base;
using BeatMapParser.Map.Difficulty.V3.Event;
using System.Collections.Generic;
using System.Linq;

namespace BeatMapParser.VNJS
{
    public class VNJSInfo
    {
        public float baseNjs { get; set; }
        public List<NjsEvent> njsEvents { get; set; }

        public VNJSInfo(float baseNjs, List<NjsEvent> njsEvents)
        {
            this.baseNjs = baseNjs;
            this.njsEvents = njsEvents.OrderBy(x => x.Seconds).ToList();
        }

        public float NjsAtSeconds(float seconds)
        {
            NjsEvent lastEvent = njsEvents.LastOrDefault(x => x.Seconds <= seconds);
            NjsEvent nextEvent = njsEvents.FirstOrDefault(x => x.Seconds > seconds);

            if (lastEvent is null) return baseNjs;
            if (nextEvent is null || nextEvent.Easing == -1) return baseNjs + lastEvent.Delta;

            var t = (seconds - lastEvent.Seconds) / (nextEvent.Seconds - lastEvent.Seconds);
            t = (float)BeatSaberEasings.Ease(t, (BeatSaberEasingType)nextEvent.Easing);
            var njsOffset = lastEvent.Delta + t * (nextEvent.Delta - lastEvent.Delta);
            return baseNjs + njsOffset;
        }

        public void CalculateAllObjectNjs(List<BeatmapGridObject> objects)
        {
            foreach (var obj in objects)
            {
                obj.njs = NjsAtSeconds(obj.Seconds);
            }
        }
    }
}
