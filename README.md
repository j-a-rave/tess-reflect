# tess-reflect

This project includes a tesselation/displacement shader for Unity, as well as a script which dynamically generates cubemaps to reflect an object's surroundings. The result is a sort of funhouse-mirror effect that works well on rounded Unity primitives.

![tess-reflect](tessreflect.gif?raw=true)

To get the full effect, a GameObject must have both CubemapRender.cs and a material using TessReflect.shader on it.

This was created for Unity 5.1.1f1. It can still be used as-is in that version, but might use some obsolete APIs for the most recent Unity.
