namespace Xardas.MLAgents.Configuration.Fileformat
{
    public static class ConfigText
    {
        //Main
        public const string behaviorsText = "behaviors";
        public const string trainerTypeText = "trainer_type";
        public const string summaryFreqText = "summary_freq";
        public const string timeHorizonText = "time_horizon";
        public const string maxStepsText = "max_steps";
        public const string keepCheckpointsText = "keep_checkpoints";
        public const string checkpointIntervalText = "checkpoint_interval";
        public const string threadedText = "threaded";
        public const string environmentParametersText = "environment_parameters";

        //Hyperparameters
        public const string hyperparametersText = "hyperparameters";

        public const string learningRateText = "learning_rate";
        public const string batchSizeText = "batch_size";
        public const string bufferSizeText = "buffer_size";
        public const string learningRateScheduleText = "learning_rate_schedule";

        //Hyperparameters - PPO and POCA specific Configurations
        public const string betaText = "beta";
        public const string epsilonText = "epsilon";
        public const string betaScheduleText = "beta_schedule";
        public const string epsilonScheduleText = "epsilon_schedule";
        public const string lambdText = "lambd";
        public const string numEpochText = "num_epoch";

        //Hyperparameters - SAC-specific Configurations
        public const string bufferInitStepsText = "buffer_init_steps";
        public const string initEntcoefText = "init_entcoef";
        public const string saveReplayBufferText = "save_replay_buffer";
        public const string tauText = "tau";
        public const string stepsPerUpdateText = "steps_per_update";
        public const string rewardSignalNumUpdateText = "reward_signal_num_update";

        //Network Settings
        public const string networkSettingsText = "network_settings";

        public const string hiddenUnitsText = "hidden_units";
        public const string numLayersText = "num_layers";
        public const string normalizeText = "normalize";
        public const string visEncodeTypeText = "vis_encode_type";
        public const string conditioningTypeText = "conditioning_type";

        public const string memoryText = "memory";
        public const string memorySizeText = "memory_size";
        public const string sequenceLengthText = "sequence_length";

        //Reward Signals
        public const string rewardSignalsText = "reward_signals";

        public const string extrinsicRewardText = "extrinsic";
        public const string gammaText = "gamma";
        public const string strengthText = "strength";

        public const string curiosityRewardText = "curiosity";

        public const string gailRewardText = "gail";
        public const string demoPathText = "demo_path";
        public const string useActionsText = "use_actions";
        public const string useVailText = "use_vail";

        public const string rndRewardText = "rnd";

        //Behavioral Cloning
        public const string behavioralCloningText = "behavioral_cloning";
        public const string stepsText = "steps";
        public const string samplesPerUpdateText = "samples_per_update";

        //Self play
        public const string selfPlayText = "self_play";

        public const string saveStepsText = "save_steps";
        public const string teamChangeText = "team_change";
        public const string swapStepsText = "swap_steps";
        public const string playAgainstLatestModelRatioText = "play_against_latest_model_ratio";
        public const string windowText = "window";
        public const string initialEloText = "initial_elo";

        //Environment Parameters

        //Sampler
        public const string samplerTypeText = "sampler_type";
        public const string samplerParametersText = "sampler_parameters";
        public const string minValueText = "min_value";
        public const string maxValueText = "max_value";
        public const string meanText = "mean";
        public const string stDevText = "st_dev";
        public const string intervalsText = "intervals";
        public const string curriculumText = "curriculum";
        public const string nameText = "name";
        public const string valueText = "value";
        public const string completionCriteriaText = "completion_criteria";
        public const string measureText = "measure";
        public const string behaviorText = "behavior";
        public const string signalSmoothingText = "signal_smoothing";
        public const string minLessonLengthText = "min_lesson_length";
        public const string thresholdText = "threshold";
    }
}