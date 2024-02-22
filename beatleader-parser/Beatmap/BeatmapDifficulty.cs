using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace beatleader_parser.Beatmap
{
    public class BeatmapDifficulty
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
        [JsonProperty(PropertyName = "customData")]
        public JObject CustomData { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BeatmapDifficulty beatmap &&
                   Version == beatmap.Version &&
                   EqualityComparer<List<BpmEvent>>.Default.Equals(BpmEvents, beatmap.BpmEvents) &&
                   EqualityComparer<List<RotationEvent>>.Default.Equals(RotationEvents, beatmap.RotationEvents) &&
                   EqualityComparer<List<Note>>.Default.Equals(Notes, beatmap.Notes) &&
                   EqualityComparer<List<Bomb>>.Default.Equals(Bombs, beatmap.Bombs) &&
                   EqualityComparer<List<Obstacle>>.Default.Equals(Obstacles, beatmap.Obstacles) &&
                   EqualityComparer<List<Arc>>.Default.Equals(Arcs, beatmap.Arcs) &&
                   EqualityComparer<List<Chain>>.Default.Equals(Chains, beatmap.Chains) &&
                   EqualityComparer<List<Waypoint>>.Default.Equals(Waypoints, beatmap.Waypoints) &&
                   EqualityComparer<List<BasicEvent>>.Default.Equals(BasicBeatmapEvents, beatmap.BasicBeatmapEvents) &&
                   EqualityComparer<List<ColorBoostEvent>>.Default.Equals(ColorBoostBeatmapEvents, beatmap.ColorBoostBeatmapEvents) &&
                   EqualityComparer<List<EventGroup<GroupColorLane>>>.Default.Equals(LightColorEventBoxGroups, beatmap.LightColorEventBoxGroups) &&
                   EqualityComparer<List<EventGroup<GroupMovementLane<GroupRotationEvent>>>>.Default.Equals(LightRotationEventBoxGroups, beatmap.LightRotationEventBoxGroups) &&
                   EqualityComparer<List<EventGroup<GroupMovementLane<GroupTranslationEvent>>>>.Default.Equals(LightTranslationEventBoxGroups, beatmap.LightTranslationEventBoxGroups) &&
                   EqualityComparer<List<FxEventGroup>>.Default.Equals(VfxEventBoxGroups, beatmap.VfxEventBoxGroups) &&
                   EqualityComparer<FxEventsCollection>.Default.Equals(FxEventsCollection, beatmap.FxEventsCollection) &&
                   EqualityComparer<BasicEventTypesWithKeywords>.Default.Equals(BasicEventTypesWithKeywords, beatmap.BasicEventTypesWithKeywords) &&
                   UseNormalEventsAsCompatibleEvents == beatmap.UseNormalEventsAsCompatibleEvents &&
                   EqualityComparer<JObject>.Default.Equals(CustomData, beatmap.CustomData);
        }

        public override int GetHashCode()
        {
            int hashCode = 1381987626;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BpmEvent>>.Default.GetHashCode(BpmEvents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<RotationEvent>>.Default.GetHashCode(RotationEvents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Note>>.Default.GetHashCode(Notes);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Bomb>>.Default.GetHashCode(Bombs);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Obstacle>>.Default.GetHashCode(Obstacles);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Arc>>.Default.GetHashCode(Arcs);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Chain>>.Default.GetHashCode(Chains);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Waypoint>>.Default.GetHashCode(Waypoints);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<BasicEvent>>.Default.GetHashCode(BasicBeatmapEvents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<ColorBoostEvent>>.Default.GetHashCode(ColorBoostBeatmapEvents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<EventGroup<GroupColorLane>>>.Default.GetHashCode(LightColorEventBoxGroups);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<EventGroup<GroupMovementLane<GroupRotationEvent>>>>.Default.GetHashCode(LightRotationEventBoxGroups);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<EventGroup<GroupMovementLane<GroupTranslationEvent>>>>.Default.GetHashCode(LightTranslationEventBoxGroups);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<FxEventGroup>>.Default.GetHashCode(VfxEventBoxGroups);
            hashCode = hashCode * -1521134295 + EqualityComparer<FxEventsCollection>.Default.GetHashCode(FxEventsCollection);
            hashCode = hashCode * -1521134295 + EqualityComparer<BasicEventTypesWithKeywords>.Default.GetHashCode(BasicEventTypesWithKeywords);
            hashCode = hashCode * -1521134295 + UseNormalEventsAsCompatibleEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<JObject>.Default.GetHashCode(CustomData);
            return hashCode;
        }
    }
}
