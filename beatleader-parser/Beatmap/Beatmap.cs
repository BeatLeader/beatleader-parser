using beatleader_parser.Beatmap.Events.V3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class Beatmap
    {
        public string Version { get; set; }
        [JsonProperty(PropertyName = "bpmEvents")]
        public List<BpmEvent> BpmEvents { get; set; }
        [JsonProperty(PropertyName = "rotationEvents")]
        public List<RotationEvent> RotationEvents { get; set; }
        [JsonProperty(PropertyName = "colorNotes")]
        public List<Note> Notes { get; set; }
        [JsonProperty(PropertyName = "bombNotes")]
        public List<Bomb> Bombs { get; set; }
        [JsonProperty(PropertyName = "obstacles")]
        public List<Obstacle> Obstacles { get; set; }
        [JsonProperty(PropertyName = "sliders")]
        public List<Arc> Arcs { get; set; }
        [JsonProperty(PropertyName = "burstSliders")]
        public List<Chain> Chains { get; set; }
        [JsonProperty(PropertyName = "waypoints")]
        public List<Waypoint> Waypoints { get; set; }
        [JsonProperty(PropertyName = "basicBeatmapEvents")]
        public List<BasicEvent> BasicBeatmapEvents { get; set; }
        [JsonProperty(PropertyName = "colorBoostBeatmapEvents")]
        public List<ColorBoostEvent> ColorBoostBeatmapEvents { get; set; }
        [JsonProperty(PropertyName = "lightColorEventBoxGroups")]
        public List<EventGroup<GroupColorLane>> LightColorEventBoxGroups { get; set; }
        [JsonProperty(PropertyName = "lightRotationEventBoxGroups")]
        public List<EventGroup<GroupMovementLane<GroupRotationEvent>>> LightRotationEventBoxGroups { get; set; }
        [JsonProperty(PropertyName = "lightTranslationEventBoxGroups")]
        public List<EventGroup<GroupMovementLane<GroupTranslationEvent>>> LightTranslationEventBoxGroups { get; set; }
        [JsonProperty(PropertyName = "vfxEventBoxGroups")]
        public List<FxEventGroup> VfxEventBoxGroups { get; set; }
        [JsonProperty(PropertyName = "_fxEventsCollection")]
        public FxEventsCollection FxEventsCollection { get; set; }
        [JsonProperty(PropertyName = "basicEventTypesWithKeywords")]
        public BasicEventTypesWithKeywords BasicEventTypesWithKeywords { get; set; }
        [JsonProperty(PropertyName = "useNormalEventsAsCompatibleEvents")]
        public bool UseNormalEventsAsCompatibleEvents { get; set; }
        // TODO: CustomData

    }
}
