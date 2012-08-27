arch
====

Arch is my personal XNA helper library--sort of a superset of my other XNA
libraries, which I no longer use. Arch is written with an eye towards
MonoGame compatibility, but has not yet been extended to support MonoGame.

The big features of Arch, roughly in order of how handy I find them, are as
follows:

* HiDPI/large screen support - the game supports two virtual resolutions
(1280x720 and 2560x1440) and intelligently handles @2x resources; if you
draw through the included IDrawable types, it'll handle scaling according
to the 1280x720 grid.
* BMFont bitmap fonts.
* Event-based input handling.
* Stack-based state machine, with percolation of updates, draws, and input.

Known issues:

* While texture objects will be scaled up or down based on their scale (@2x
or regular) to work on regular and HiDPI displays, you must have a @2x and
regular font or things get weird.
* The content system is currently hard-coded to the resources I needed. That
will probably change at some point (I do have Contentious lying around, after
all, though that'd need some work).
* The content system's texture atlas generator requires a paid version of
TexturePackerPro. You can get around this by using the free version and taking
out the MaxRects parameter in the python script.
* There's some gross reflection being used to load MP3s for use in the XNA
Song class. Blame the idiots who made its public API crap. When I do the
MonoGame version I'll make sure to make it compatible.
* There may be some weirdness in TextureRegions with regard to rotation and
sprite scaling; I haven't tested it thoroughly.


If you use it, drop me a line. This is primarily a project for my own use,
but it'd be cool if others find it useful.

Ed Ropple
ed+arch@edropple.com
