## Review of ArgParser
# First impression

The README file in the project repository is well structured and contains nearly all key information about the API.
There are some public methods like `GenerateHelp()` which are not mentioned but this is just a detail.
I like the simple example in README file even though it maybe misses some comments.
It clearly demonstrates simple use case and helps with orientation in how the basic concepts of your API are used.

# API review

The object oriented approach to option and argument definition is definitely interesting.
It might be a bit unclear for someone new to C# because it relies on knowledge of inheritance and stuff
but if you are more experienced or programming something similar to the example from README than it is OK I guess.

I think that this library overall satisfies all the requirements from *task-1*.
Maybe I wasn't quite sure how to define that an option has some mandatory and some optional arguments.
It looks like your API only allows to set an option as mandatory but maybe it somehow affects it's arguments
and I just didn't find it in the documentation.

I also find the way of defining argument order a bit confusing and I haven't found a way how to define option 
which takes more than one argument and how to set order of these arguments.
But I really appreciate that you provide a pretty simple  possibility how to define your own option type with it's
own parsing implementation.

# Writing numactl

Implementing of the *numactl* program using your library was quite easy. Even though there were some parts which
I'm not sure If I implemented the right way because the task was a bit unclear in some ways.
I did kind of missed a way how to simply get all the parsed data to one data-structure which I could for example iterate over
but that's just an edge use case.

# Detailed comments

The overall code style of your library is OK and consistent form my point of view.
I would maybe more utilize the possibilities which come from the `interface` concept in C#.
Even though some parts like predefined option and argument types are missing comments
the code itself is well documented both in the code and in the external documentation.

Due to the small scale of this application I can't really think of any other comments, but I hope that my review will at least a bit helpful.



