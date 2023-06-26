namespace Xardas.MLAgents.Configuration.Fileformat
{
    public static class ConfigTooltip
    {
        //Behavior
        public const string behaviorName = "You have to add this to the Behavior Parameters.";
        public const string summaryFreq = "Number of experiences that needs to be collected before generating and displaying training statistics.";
        public const string timeHorizon = "How many steps of experience to collect per-agent before adding it to the experience buffer.\nTypical range: 32 - 2048";
        public const string maxSteps = "Total number of steps that must be taken in the environment before ending the training process.\nTypical range: 5e5 - 1e7";
        public const string keepCheckpoints = "The maximum number of model checkpoints to keep.";
        public const string checkpointInterval = "The number of experiences collected between each checkpoint by the trainer.";
        public const string threaded = "Allow environments to step while updating the model.\nLeave setting to false when using self-play.";

        //Hyperparameters
        public const string learningRate = "Initial learning rate for gradient descent.\nTypical range: 1e-5 - 1e-3";
        public const string batchSize = "Number of experiences in each iteration of gradient descent. This should always be multiple times smaller than buffer size.\nTypical range\nContinuous - PPO: 512 - 5120\nContinuous - SAC: 128 - 1024\nDiscrete, PPO & SAC: 32 - 512";
        public const string overwriteBufferSize = "Default:\nPPO/POCA: 10240\nSAC: 50000";
        public const string bufferSize = "PPO/POCA: Number of experiences to collect before updating the policy model.\nSAC: The max size of the experience buffer.\nTypical range\nPPO: 2048 - 409600\nSAC: 50000 - 1000000";
        public const string overwriteLearningRateSchedule = "Default:\nPPO/POCA: linear\nSAC: constant";
        public const string learningRateSchedule = "Linear: it decays the learning_rate linearly, reaching 0 at max_steps.\nConstant: it keeps the learning rate constant for the entire training run.";

        //Hyperparameters - PPO and POCA specific Configurations
        public const string beta = "Strength of the entropy regularization, which makes the policy \"more random\".\nTypical range: 1e-4 - 1e-2";
        public const string epsilon = "Influences how rapidly the policy can evolve during training.\nTypical range: 0.1 - 0.3";
        public const string overwriteBetaSchedule = "Default: learning rate schedule";
        public const string betaSchedule = "Determines how beta changes over time.";
        public const string overwriteEpsilonSchedule = "Default: learning rate schedule";
        public const string epsilonSchedule = "Determines how epsilon changes over time (PPO only).";
        public const string lambd = " Regularization parameter (lambda) used when calculating the Generalized Advantage Estimate.\nTypical range: 0.9 - 0.95";
        public const string numEpoch = "Number of passes to make through the experience buffer when performing gradient descent optimization.\nTypical range: 3 - 10";

        //Hyperparameters - SAC-specific Configurations
        public const string bufferInitSteps = "Number of experiences to collect into the buffer before updating the policy model.\nTypical range: 1000 - 10000";
        public const string initEntcoef = "How much the agent should explore in the beginning of training.\nTypical range: (Continuous): 0.5 - 1.0; (Discrete): 0.05 - 0.5";
        public const string saveReplayBuffer = "Whether to save and load the experience replay buffer as well as the model when quitting and re-starting training. ";
        public const string tau = "How aggressively to update the target network used for bootstrapping value estimation in SAC.";
        public const string stepsPerUpdate = "Average ratio of agent steps (actions) taken to updates made of the agent's policy.\nTypical range: 1 - 20";
        public const string overwriteRewardSignalNumUpdate = "Default: steps per update";
        public const string rewardSignalNumUpdate = "Number of steps per mini batch sampled and used for updating the reward signals.";

        //NetworkSettings
        public const string hiddenUnits = "Number of units in the hidden layers of the neural network.\nTypical range: 32 - 512";
        public const string numLayers = "The number of hidden layers in the neural network.\nTypical range: 1 - 3";
        public const string normalize = "Whether normalization is applied to the vector observation inputs.";
        public const string visEncodeType = "Encoder type for encoding visual observations.";
        public const string conditioningType = "Conditioning type for the policy using goal observations.\nNone: it treats the goal observations as regular observations.\nHyper: it uses a HyperNetwork with goal observations as input to generate some of the weights of the policy.";

        public const string memorySize = "Size of the memory an agent must keep.\nIt is required that it is divisible by 2.";
        public const string sequenceLength = "Defines how long the sequences of experiences must be while training.";


        //Reward Signals

        //Extrinsic Reward
        public const string extrinsicStrength = "Factor by which to multiply the reward given by the environment.\nTypical range: 1.00";
        public const string extrinsicGamma = "Discount factor for future rewards coming from the environment.\nTypical range: 0.8 - 0.995";

        //Curiosity Intrinsic Reward & RND Intrinsic Reward
        public const string curiosityrndStrength = "Magnitude of the curiosity reward generated by the intrinsic module.\nTypical range: 0.001 - 0.01";
        public const string curiosityrndGamma = "Discount factor for future rewards.\nTypical range: 0.8 - 0.995";
        public const string curiosityrndLearningRate = "Learning rate used to update the intrinsic curiosity module.\nTypical range: 1e-5 - 1e-3";


        //GAIL Intrinsic Reward
        public const string gailStrength = "Factor by which to multiply the raw reward.\nTypical range: 0.01 - 1.0";
        public const string gailGamma = "Discount factor for future rewards.\nTypical range: 0.8 - 0.9";
        public const string gailDemoPath = "The path to your .demo file or directory of .demo files. (Required)";
        public const string gailLearningRate = "Learning rate used to update the discriminator.\nTypical range: 1e-5 - 1e-3";
        public const string gailUseActions = "Determines whether the discriminator should discriminate based on both observations and actions, or just observations.";
        public const string gailUseVail = "Enables a variational bottleneck within the GAIL discriminator.";


        //Behavioral Cloning
        public const string behavioralCloningDemoPath = "The path to your .demo file or directory of .demo files. (Required)";
        public const string behavioralCloningStrength = "Learning rate of the imitation relative to the learning rate of PPO, and roughly corresponds to how strongly we allow BC to influence the policy.\nTypical range: 0.1 - 0.5";
        public const string behavioralCloningSteps = "It corresponds to the training steps over which Behavioral Cloning is active.";
        public const string behavioralCloningOverwriteBatchSize = "Default: batch size of trainer";
        public const string behavioralCloningBatchSize = "Number of demonstration experiences used for one iteration of a gradient descent update.\nTypical range\nContinuous: 512 - 5120\nDiscrete: 32 - 512";
        public const string behavioralCloningOverwriteNumEpoch = "Default: num epoch of trainer";
        public const string behavioralCloningNumEpoch = "Number of passes through the experience buffer during gradient descent.\nTypical range: 3 - 10";
        public const string behavioralCloningSamplesPerUpdate = "Maximum number of samples to use during each imitation update.\nTypical range: Buffer Size";

        //Self play
        public const string saveSteps = "Number of trainer steps between snapshots.\nTypical range: 10000 - 100000";
        public const string overwriteTeamChange = "Default value: 5 * save steps";
        public const string teamChange = "Number of trainer_steps between switching the learning team.\nTypical range: 4x-10x where x=Save steps";
        public const string swapSteps = "Number of ghost steps (not trainer steps) between swapping the opponents policy with a different snapshot.\nTypical range: 10000 - 100000";
        public const string playAgainstLatestModelRatio = "Probability an agent will play against the latest opponent policy.\nTypical range: 0.0 - 1.0";
        public const string window = "Size of the sliding window of past snapshots from which the agent's opponents are sampled.\nTypical range: 5 - 30";

        //Environment Parameters

        //Sampler
        public const string stDev = "Standard deviation";

        //Curriculum
        public const string completionCriteria = "Which determines what needs to happen in the simulation before the lesson can be considered complete.\nThe last lesson's completion criteria won't be in the yaml file.";
        public const string measure = "What to measure learning progress, and advancement in lessons by.\nReward: it uses a measure of received reward.\nProgress: it uses the ratio of steps/max_steps\nElo: it is available only for self-play situations and uses Elo score as a curriculum completion measure.";
        public const string behavior = "Specifies which behavior is being tracked.";
        public const string threshold = "Determines at what point in value of `measure` the lesson should be increased.";
        public const string minLessonLength = "The minimum number of episodes that should be completed before the lesson can change.";
        public const string signalSmoothing = "Whether to weight the current progress measure by previous values.";
        public const string requireReset = "Whether changing lesson requires the environment to reset.";
    }
}