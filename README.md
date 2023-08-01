
# Playhome image based lighting & deferred shading
## Features
1. Load cubemaps into the game
2. Deferred shading
3. Tessellation
4. Percentage-closer soft shadow

## Install and use
1. install the latest [IPA](https://github.com/Eusth/IPA/releases);
2. [Download the latest release](https://bitbucket.org/plastics/myphipaplugins/downloads/), extract everything into the game folder;
3. [Download Cubemaps](https://mega.nz/#F!ATJQRDBY!c69iE9FSwyE0oAY9vWEMWw), put them in "cubemaps" folder;
4. Press `F5` to start the window;
5. If you want to create your own cubemaps, check the [wiki](https://bitbucket.org/plastics/myphipaplugins/wiki/Home).

## Credits
HDRIs are from

https://hdrihaven.com/

https://www.hdrvault.com/

https://www.assetstore.unity3d.com/#!/content/72511

https://zbyg.deviantart.com

# Playhome custom texture patch
## Feature
Support the game, the studio and VR program.

Add 2K & 4K & 8K makeup and tattoo support. Introduce the new makeups and tattoos mod standard, a texture size indicator is put in the offset coordinates.

2K makeups and tattoos: coordinates = the real offset coordinates + (10000, 10000)

4K makeups and tattoos: coordinates = the real offset coordinates + (20000, 20000)

8K makeups and tattoos: coordinates = the real offset coordinates + (30000, 30000)

e.g. I made a miku tattoo in 4096x4096 resolution, and crop to 160x160 with offset (2172,3532), the new offset should be (22172,23532)

I would recommend face texture in 2K and body textures in 4K, because all custom textures on the skin are also limited by the base skin diffuse map, which is 4K for vanilla body and 2K for vanilla face. So a 4K face makeup or tattoo will be downscaled to 2K, unless someone made new 4K face skin types.

## Install
1. Install IPA;
2. Extract and put everything in the game folder, the 4K miku tattoo should appear at the right place.

# Playhome mirror helper

This plugin will change the rendertexture resolution of mirrors and enable HDR rendering automatically.

Press `F6` in a scene with a mirror to access detail configs. 
