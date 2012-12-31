# RDSFeed

RDSFeed is an application for outputting timely text statements from multiple information sources for use 
in text presentation systems. Actually, "RDS" (an acronym for Radio Data System) is a misnomer here -- I 
use this application for sending data over RDS. However, it can just as easily work for other systems 
that can accept text, such as scrolling LED readers.


## Getting started

==> [RDSFeed installer](https://dl.dropbox.com/u/7320109/RDSFeedInstall.msi) The installer adds RDSFeed 
and all supporting files on your PC.

There is also a [quickstart](https://github.com/greenlakexmas/RDSFeed/blob/master/QUICKSTART.md) to make you
more informed.

And of course, the entire source for RDSFeed is available here.

## What it does

RDSFeed is an operational container for producing text statements in a timely manner. The RDSFeed application
produces a text file as its output, containing a text string. The application continually updates this text file, 
replacing the content of the file with new text statements. What those statements contain are completely 
configurable through template configuration. This type of operation is useful in applications such as RDS.


The output of RDSFeed is a text file. This text file will contain a statement of some sort, which is controlled
through configuration files. When the RDSFeed application runs, an output text file is produced and continually 
updated by the application (as long as it is running.) In graphical form:

```
------------
|   RDS    |   ==== >>>   output.txt
|   Feed   |
------------
```

Many systems that can accept text string input, such as FM transmitters that support RDS or scrolling LED devices, 
will monitor a text file that serves as the input source. RDSFeed is constructed to create such an output file.


## How it works

RDSFeed processing flow consists of:

* Reading the local rds.config file for operational settings
* Initializing and starting each data source
* Writing outbound text messages created by the data sources

RDSFeed is an operational container that executes each data source independently and concurrently to
generate text output. The data sources are responsible for formatting, gathering and submitting text 
messages back to the main application thread. The main application thread is responsible for pushing 
those text messages to the output channel.

## Internals

RDSFeed is both configurable and extensible.

#### Configuration

Configuration for RDSFeed is contained in a file named "rds.config". It lives in the same directory as the RDSFeed
application. RDSFeed looks for this file on startup and uses it as the basis for operation.
The source code contains an 
[example configuration file](https://github.com/greenlakexmas/RDSFeed/blob/master/rds.config.example "configuration file"), 
which we'll refer to here.

There are several sections to rds.config that are important to successful operation of RDSFeed. Here's a breakdown
of those sections:

* RefreshInterval - the time duration loop speed, measured in seconds. This setting will 
refresh a new message every 30 seconds.
* MergeFile - the target output file for RDSFeed.
* Sources - The different sources of text information. Each source is defined here.

Each Source consists of the following:

* Name and Type - attributes of the Source that identify it (by name) 
* RefreshInterval - the time duration loop speed, measured in seconds. This setting will 
refresh a new message every 30 seconds.
* Priority - the importance of the message. Setting is either Immediate or General.
* Constructor - parameters used to start up the data source.
* Templates - formatted text strings mapped to programmatic methods to produce text output
* Options - supplemental parameters that may optionally be needed for a data source

#### Extensibility

The RDSFeed application is an operational container that dynamically loads data sources via configuration. 
Additional data sources can be constructed implementing the 
[IDataSource](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/IDataSource.cs) interface. The
existing data sources use a 
[base implementation](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/BaseDataSource.cs) 
for consistency and code re-use. If you wish to add a data source, look to the existing implementations
for guidance.
