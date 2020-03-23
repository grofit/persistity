# Transformers

These are simply ways to transform objects from one layout to another, its kind of like a very simple automapper.

In a lot of scenarios you will be dealing with complex objects that cannot be directly serialized or sent to endpoints, so these will provide you the means to take the complex data and simplify it, or just cull the bits you dont need.

## Transform to and From

Originally transformers contained 2 methods, a to and a from method, but since then we have streamlined it to just be a `Transform` method, and in the cases when you want to go to and from a type, just implement 2 transformers one for each direction.