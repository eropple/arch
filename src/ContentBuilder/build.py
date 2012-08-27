#! /usr/bin/env python

# Handles the content build step for ArchLib. Based on Anaeon's content
# system, itself based on Archos's content system...this thing has been
# around a while, this is rewrite four or something.
#
# -eropple
# 18 Jul 2012

import os
import sys
import shutil
import time
import subprocess
from datetime import datetime

print "ArchLib Content Build"
print "====================="
print

SCRIPTS_DIR = os.path.join(os.path.dirname(__file__), "scripts")

if len(sys.argv) < 3:
    print "Usage: %s CONTENT_ROOT ASSET_ROOT" % os.path.basename(sys.argv[0])
    print "       CONTENT_ROOT is the parent directory of source and prebuilt."
    print "       ASSET_ROOT is where you want the compiled content to go."
    sys.exit(1)

CONTENT_ROOT = os.path.realpath(sys.argv[1])
if not os.path.exists(CONTENT_ROOT) or not os.path.isdir(CONTENT_ROOT):
    print
    print
    print "ERROR: Content root '%s' does not exist or is not a directory." \
            % CONTENT_ROOT

ASSET_ROOT = os.path.realpath(sys.argv[2])
if os.path.exists(ASSET_ROOT) and not os.path.isdir(ASSET_ROOT):
    print
    print
    print "ERROR: Asset root '%s' already exists but is not a directory." \
            % ASSET_ROOT
    print
    sys.exit(2)

print "Content root: " + CONTENT_ROOT
print "Asset root:   " + ASSET_ROOT

start_time = time.time()

old_dir = ASSET_ROOT + "_old"

if os.path.exists(old_dir):
    print " - Cleaning up '%s'..." % old_dir
    print
    shutil.rmtree(old_dir)

if os.path.exists(ASSET_ROOT):
    print " - Copying '%s' to '%s'..." % (ASSET_ROOT, old_dir)
    print
    shutil.move(ASSET_ROOT, old_dir)

print " - Creating '%s' with prebuilt assets..." % ASSET_ROOT
print
prebuilt_dir = os.path.join(CONTENT_ROOT, "prebuilt")
shutil.copytree(prebuilt_dir, ASSET_ROOT)

print " - Calling python files in 'scripts' to process source files..."
print

failures = False

os.chdir(SCRIPTS_DIR)
for file in os.listdir("."):
    if file.endswith(".py"):
        filename = os.path.split(file)[1]
        print ">>> %s" % filename
        app_args = ["python", file, ASSET_ROOT, CONTENT_ROOT]
        ret = subprocess.call(app_args, shell=True)
        if ret != 0:
            print "!!! WARNING: '%s' EXITED WITH ERROR CODE %d!" % (filename, \
                    ret)
            failures = True
        print

end_time = time.time()

print
print "start time: %s" % time.strftime("%d %b %Y %I:%M:%S %p", \
        time.localtime(start_time))
print "  end time: %s" % time.strftime("%d %b %Y %I:%M:%S %p", \
        time.localtime(end_time))
print "total time: %s seconds" % str(end_time - start_time)

if failures:
    print
    print "!!! WARNING !!!"
    print "At least one processing script failed. Check the logs!"
    sys.exit(254)
