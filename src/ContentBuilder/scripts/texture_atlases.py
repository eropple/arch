#! /usr/bin/env python

# Walks each subdirectory within ATLAS_ROOT. Each subdirectory within
# ATLAS_ROOT is its own texture atlas. For example, ATLAS_ROOT/some_atlas
# is the "some_atlas" atlas and all files within it wiill be compacted into
# ASSET_ROOT/atlases/some_atlas.png and ASSET_ROOT/atlases/some_atlas.txt.
#
# -eropple
# 18 Jul 2012

import os
import sys
import subprocess
import platform

SPRITE_SHEET_FORMAT = "xml"

SYSTEM_TYPE = platform.uname()[0]

if SYSTEM_TYPE == "Windows":
    TEXTURE_PACKER = "C:\\Program Files (x86)\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe"
elif SYSTEM_TYPE == "Darwin" or SYSTEM_TYPE == "Linux":
    TEXTURE_PACKER = "/usr/local/bin/TexturePacker"
else:
    print "Couldn't understand system type."
    sys.exit(100)

ASSET_ROOT = sys.argv[1]
CONTENT_ROOT = sys.argv[2]

ATLAS_ROOT = os.path.join(CONTENT_ROOT, "source", "Atlases")
PNG_OUTPUT = os.path.join(ASSET_ROOT, "Textures", "Atlases")
ATL_OUTPUT = os.path.join(ASSET_ROOT, "Atlases")

error = False

os.makedirs(ATL_OUTPUT)
os.makedirs(PNG_OUTPUT)

for file in os.listdir(ATLAS_ROOT):
    os.chdir(ATLAS_ROOT)
    if os.path.isdir(file):
        full_dir = os.path.join(ATLAS_ROOT, file)

        sys.stdout.write(file + " ...")
        atl_img = os.path.join(PNG_OUTPUT, "%s.png" % file)
        atl_txt = os.path.join(ATL_OUTPUT, "%s.atlas" % file)

        if SYSTEM_TYPE == "Windows":
            cmd = [ TEXTURE_PACKER, "--quiet", \
                    "--algorithm", "MaxRects", \
                    "--trim-sprite-names", \
                    "--format", SPRITE_SHEET_FORMAT, \
                    "--disable-rotation", "--no-trim", \
                    "--sheet", atl_img, "--data", atl_txt ]

            # if the atlas name has -hd or @2x in it, we want
            # to automatically build a standard-definition version.
            if file.endswith("-hd") or file.endswith("@2x"):
                cmd.insert(1, "--auto-sd")

            os.chdir(full_dir)
            for img in os.listdir(full_dir):
                if img.endswith(".png"):
                    cmd.append(img)
        else:
            cmd = [ TEXTURE_PACKER + " --quiet --algorithm MaxRects " + \
                    "--trim-sprite-names --format " + SPRITE_SHEET_FORMAT + " " + \
                    "--disable-rotation --no-trim " + \
                    "--sheet " + atl_img + " --data " + atl_txt ]

            # if the atlas name has -hd or @2x in it, we want
            # to automatically build a standard-definition version.
            if file.endswith("-hd") or file.endswith("@2x"):
                cmd[0] = cmd[0].replace("--quiet", "--quiet --auto-sd")

            os.chdir(full_dir)
            for img in os.listdir(full_dir):
                if img.endswith(".png"):
                    cmd[0] = cmd[0] + " " + img


        # print cmd
        with open(os.devnull, 'w') as devnull:
            retval = subprocess.call(cmd, shell=True, \
                    stderr=devnull, stdout=devnull);

        r = retval
        if r == 0:
            if file.endswith("-hd") or file.endswith("@2x"):
                print "done"
            else:
                print "done (warning - no @2x version)"
        else:
            print "error! code %d" % r
            error = True

if error:
    sys.exit(1)
