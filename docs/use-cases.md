# Use Cases

There are quite a few common game dev use cases where a library like this could be useful:

## Saving / Loading Player Data

The most common example is taking your player data and saving/loading it.

## Sending Preferences to 3rd parties

If you use things like PlayFab or other 3rd party providers for game storage/multiplayer etc, a common use case would be where you want to collect some metadata around the players preferences or profile and sending it over to the 3rd party.

## Populating In Memory Databases

If you have a game with an inventory, you probably have a database of items (be it a flat file, an actual DB, web call etc), so you can make a pipeline to extract this data and convert it into a list of items.

## Saving Custom Levels/Content

If you have a game where users can take pictures or create their own levels and that data needs to be put somewhere, you can create a pipeline for this to take the level and store it somewhere for later.

## Editor Data Saving/Loading

Much like ScriptableObjects you can create your own notion of data that can be pushed and pulled from remote sources allowing you to create editor plugins that can provide config data at design time.

Almost all scenarios which revolve around data needing to go somewhere and come out of somewhere can be covered through a pipeline.