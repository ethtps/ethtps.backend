# Help us improve

Welcome! We're so glad you're interested in helping out with this project. We are in need of data providers for missing L2 implementations of the `IHTTPBlockInfoProvider` C# interface.

The `IHTTPBlockInfoProvider` interface is part of the background service that provides information about blocks. The interface includes methods for getting information about the latest block and blocks by either their block number or by their timestamp.

The `Block` class holds the data of a single block in the blockchain, which includes properties like the block number, transaction count, gas used, date, settled status, provider, and an array of transaction hashes.

## How to Contribute

Your task is to implement the `IHTTPBlockInfoProvider` interface for a specific L2 solution. This requires understanding the block structure of that specific L2 and how to fetch the relevant block data. 

To get started, take a look at the following key details:

### 1. BlockTimeSeconds (Optional)

`BlockTimeSeconds` is an optional property that can be used to specify the average time it takes an L2 to create a new block. If no value is provided for `BlockTimeSeconds`, the time between the last two blocks will be considered instead.

### 2. GetLatestBlockInfoAsync()

The `GetLatestBlockInfoAsync` method should return an instance of `Block` with the information of the latest mined block in the L2 solution.

### 3. GetBlockInfoAsync(int blockNumber)

The `GetBlockInfoAsync` method, when provided a block number as a parameter, should return an instance of `Block` with the information of the block that corresponds to the provided block number.

### 4. GetBlockInfoAsync(DateTime time) (Optional)

The `GetBlockInfoAsync` method, when provided a `DateTime` parameter, should return an instance of `Block` with the information of the block that was created closest to the provided timestamp. 

This method is optional but is good to have since it can be used for fetching historical data.

## Making a Pull Request

Once you've finished implementing your data provider, please make a pull request to this repository's development branch. Please include tests to demonstrate that the implementation works correctly and consider providing documentation so that others can understand your code.

Remember, your contributions make a big difference in the growth of this project and the broader community.

Thank you for your interest and we look forward to seeing your contributions!
