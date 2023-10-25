namespace Xardas.MLAgents.Configuration.Fileformat
{
    public static class ConfigText
    {
        //Default
        public const string defaultSettings = "default_settings";

        //Behavior
        public const string behaviors = "behaviors";
        public const string trainerType = "trainer_type";
        public const string summaryFreq = "summary_freq";
        public const string timeHorizon = "time_horizon";
        public const string maxSteps = "max_steps";
        public const string keepCheckpoints = "keep_checkpoints";
        public const string evenCheckpoints = "even_checkpoints";
        public const string checkpointInterval = "checkpoint_interval";
        public const string initPath = "init_path";
        public const string threaded = "threaded";
        public const string environmentParameters = "environment_parameters";

        //Hyperparameters
        public const string hyperparameters = "hyperparameters";

        public const string learningRate = "learning_rate";
        public const string batchSize = "batch_size";
        public const string bufferSize = "buffer_size";
        public const string learningRateSchedule = "learning_rate_schedule";

        //Hyperparameters - PPO and POCA specific Configurations
        public const string beta = "beta";
        public const string epsilon = "epsilon";
        public const string betaSchedule = "beta_schedule";
        public const string epsilonSchedule = "epsilon_schedule";
        public const string lambd = "lambd";
        public const string numEpoch = "num_epoch";
        public const string sharedCritic = "shared_critic";

        //Hyperparameters - SAC-specific Configurations
        public const string bufferInitSteps = "buffer_init_steps";
        public const string initEntcoef = "init_entcoef";
        public const string saveReplayBuffer = "save_replay_buffer";
        public const string tau = "tau";
        public const string stepsPerUpdate = "steps_per_update";
        public const string rewardSignalNumUpdate = "reward_signal_num_update";

        //Network Settings
        public const string networkSettings = "network_settings";

        public const string hiddenUnits = "hidden_units";
        public const string numLayers = "num_layers";
        public const string normalize = "normalize";
        public const string visEncodeType = "vis_encode_type";
        public const string conditioningType = "conditioning_type";

        public const string memory = "memory";
        public const string memorySize = "memory_size";
        public const string sequenceLength = "sequence_length";

        //Reward Signals
        public const string rewardSignals = "reward_signals";

        public const string extrinsicReward = "extrinsic";
        public const string gamma = "gamma";
        public const string strength = "strength";

        public const string curiosityReward = "curiosity";

        public const string gailReward = "gail";
        public const string demoPath = "demo_path";
        public const string useActions = "use_actions";
        public const string useVail = "use_vail";

        public const string rndReward = "rnd";

        //Behavioral Cloning
        public const string behavioralCloning = "behavioral_cloning";
        public const string steps = "steps";
        public const string samplesPerUpdate = "samples_per_update";

        //Self play
        public const string selfPlay = "self_play";

        public const string saveSteps = "save_steps";
        public const string teamChange = "team_change";
        public const string swapSteps = "swap_steps";
        public const string playAgainstLatestModelRatio = "play_against_latest_model_ratio";
        public const string window = "window";
        public const string initialElo = "initial_elo";

        //Environment Parameters

        //Sampler
        public const string samplerType = "sampler_type";
        public const string samplerParameters = "sampler_parameters";
        public const string minValue = "min_value";
        public const string maxValue = "max_value";
        public const string mean = "mean";
        public const string stDev = "st_dev";
        public const string intervals = "intervals";
        public const string curriculum = "curriculum";
        public const string name = "name";
        public const string value = "value";
        public const string completionCriteria = "completion_criteria";
        public const string measure = "measure";
        public const string behavior = "behavior";
        public const string threshold = "threshold";
        public const string minLessonLength = "min_lesson_length";
        public const string signalSmoothing = "signal_smoothing";
        public const string requireReset = "require_reset";

        //Environment settings
        public const string environmentSettings = "env_settings";
        public const string envPath = "env_path";
        public const string envArgs = "env_args";
        public const string basePort = "base_port";
        public const string numEnvs = "num_envs";
        public const string timeoutWait = "timeout_wait";
        public const string seed = "seed";
        public const string maxLifetimeRestarts = "max_lifetime_restarts";
        public const string restartsRateLimitN = "restarts_rate_limit_n";
        public const string restartsRateLimitPeriodS = "restarts_rate_limit_period_s";

        //Engine settings
        public const string engineSettings = "engine_settings";
        public const string width = "width";
        public const string height = "height";
        public const string qualityLevel = "quality_level";
        public const string timeScale = "time_scale";
        public const string targetFrameRate = "target_frame_rate";
        public const string captureFrameRate = "capture_frame_rate";
        public const string noGraphics = "no_graphics";

        //Checkpoint settings
        public const string checkpointSettings = "checkpoint_settings";
        public const string runId = "run_id";
        public const string initializeFrom = "initialize_from";
        public const string loadModel = "load_model";
        public const string resume = "resume";
        public const string force = "force";
        public const string trainModel = "train_model";
        public const string inference = "inference";

        //Torch settings
        public const string torchSettings = "torch_settings";
        public const string device = "device";
    }
}