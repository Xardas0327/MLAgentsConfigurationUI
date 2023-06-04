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
        public const string threaded = "Allow environments to step while updating the model.";

        //Hyperparameters
        public const string learningRate = "Initial learning rate for gradient descent.\nTypical range: 1e-5 - 1e-3";
        public const string batchSize = "Number of experiences in each iteration of gradient descent. This should always be multiple times smaller than buffer size.";
        public const string bufferSize = "PPO/POCA: Number of experiences to collect before updating the policy model.\nSAC: The max size of the experience buffer.";
        public const string learningRateSchedule = "Linear: it decays the learning_rate linearly, reaching 0 at max_steps.\nConstant: it keeps the learning rate constant for the entire training run.";

        //Hyperparameters - PPO and POCA specific Configurations
        public const string beta = "Strength of the entropy regularization, which makes the policy \"more random\".\nTypical range: 1e-4 - 1e-2";
        public const string epsilon = "Influences how rapidly the policy can evolve during training.\nTypical range: 0.1 - 0.3";
        public const string betaSchedule = "Determines how beta changes over time.";
        public const string epsilonSchedule = "Determines how epsilon changes over time (PPO only).";
        public const string lambd = " Regularization parameter (lambda) used when calculating the Generalized Advantage Estimate.\nTypical range: 0.9 - 0.95";
        public const string numEpoch = "Number of passes to make through the experience buffer when performing gradient descent optimization.\nTypical range: 3 - 10";

        //Hyperparameters - SAC-specific Configurations
        public const string bufferInitSteps = "Number of experiences to collect into the buffer before updating the policy model.\nTypical range: 1000 - 10000";
        public const string initEntcoef = "How much the agent should explore in the beginning of training.\nTypical range: (Continuous): 0.5 - 1.0; (Discrete): 0.05 - 0.5";
        public const string saveReplayBuffer = "Whether to save and load the experience replay buffer as well as the model when quitting and re-starting training. ";
        public const string tau = "How aggressively to update the target network used for bootstrapping value estimation in SAC.";
        public const string stepsPerUpdate = "Average ratio of agent steps (actions) taken to updates made of the agent's policy.\nTypical range: 1 - 20";
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

    }
}