# delold

## What is it?

delold is a simple tool designed to delete any files older than a specified number of days within a directory. Options include recursing into subdirectories, and deleting any empty subdirectories after file deletion.

delold was originally written to clean up periodic photos taken by my home security camera, and I have since adopted it for use on a number of servers to clean up temp and cache directories.

## Usage

    delold <days> <path> [-r|-e] [-c] [-t]

Options:

* &lt;days&gt; (Required) - Minimum age of file to delete, in whole days. Specifying 0 will delete everything.
* &lt;path&gt; (Required) - Path of directory to clean out.
* -r - Recurse into subdirectories underneath <path> when finding and deleting files.
* -e - When recursing into subdirectories, also delete any empty subdirectories encountered after file deletion. The directory specified by <path> will not be deleted, even if empty. Note that all empty subdirectories will be deleted, irrespective of the value specified for <days>. Use of the -e option implies -r. 
* -c - Normally, delold will look at file modification times to calculate age. Specifying -c will use creation times instead.
* -t - Test mode. A list of files to be deleted will be printed, but no files will actually be deleted. Note that you may not see as many empty subdirectories being deleted when specifying the -e option, as no files are being removed first.

## License

Copyright (c) 2016 Dave Britten

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
