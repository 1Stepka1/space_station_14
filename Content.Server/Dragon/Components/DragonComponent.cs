using System.Threading;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Dragon
{
    [RegisterComponent]
    public sealed partial class DragonComponent : Component
    {


        /// <summary>
        /// For deadless dragon's :)
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("heNeedsAlive")]
        public bool HeNeedsAlive = false;



        /// <summary>
        /// For Roar dragon's :)
        /// </summary>
        public bool HeRoars = false;
        [ViewVariables(VVAccess.ReadWrite), DataField("RoarFrequency")]
        public int RoarFrequency = 30;
        private int _defaultRoarTimeDelay = 100;
        [ViewVariables(VVAccess.ReadWrite), DataField("DefaultRoarTimeDelay[DEBUG]")]
        public int DefaultRoarTimeDelay
        {
            get
            {
                return _defaultRoarTimeDelay;
            }
            set
            {
                _defaultRoarTimeDelay = value;
            }
        }

        /// <summary>
        /// If we have active rifts.
        /// </summary>
        [DataField("rifts")]
        public List<EntityUid> Rifts = new();

        public bool Weakened => WeakenedAccumulator > 0f;

        /// <summary>
        /// When any rift is destroyed how long is the dragon weakened for
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("weakenedDuration")]
        public float WeakenedDuration = 120f;

        /// <summary>
        /// Has a rift been destroyed and the dragon in a temporary weakened state?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("weakenedAccumulator")]
        public float WeakenedAccumulator = 0f;

        [ViewVariables(VVAccess.ReadWrite), DataField("riftAccumulator")]
        public float RiftAccumulator = 0f;

        /// <summary>
        /// Maximum time the dragon can go without spawning a rift before they die.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("maxAccumulator")] public float RiftMaxAccumulator = 300f;

        /// <summary>
        /// Spawns a rift which can summon more mobs.
        /// </summary>
        [DataField("spawnRiftAction")]
        public InstantAction? SpawnRiftAction;

        [ViewVariables(VVAccess.ReadWrite), DataField("riftPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string RiftPrototype = "CarpRift";

        [ViewVariables(VVAccess.ReadWrite), DataField("soundDeath")]
        public SoundSpecifier? SoundDeath = new SoundPathSpecifier("/Audio/Animals/space_dragon_roar.ogg");

        [ViewVariables(VVAccess.ReadWrite), DataField("soundRoar")]
        public SoundSpecifier? SoundRoar =
            new SoundPathSpecifier("/Audio/Animals/space_dragon_roar.ogg")
            {
                Params = AudioParams.Default.WithVolume(0.2f),
            };

        [ViewVariables(VVAccess.ReadWrite), DataField("soundAlterRoar")]
        public SoundSpecifier? SoundAlterRoar =
            new SoundPathSpecifier("/Audio/Animals/alternative_space_dragon_roar.ogg")
            {
                Params = AudioParams.Default.WithVolume(1f),
            };
    }

    public sealed partial class DragonDevourActionEvent : EntityTargetActionEvent {}

    public sealed partial class DragonSpawnRiftActionEvent : InstantActionEvent {}

    public sealed class DragonRoarActionEvent : InstantActionEvent { }
}
