# RDSFeed Quickstart

RDSFeed is an operational container for producing text statements in a timely manner. It is designed to
be both configurable and extensible. This Quickstart is all about using the existing application and
configuring it to run for yourself.

## Instructions
1. Configure data sources to be used
2. Configure interoperating applications
3. Run RDSFeed.exe

## Data sources

Out of the box, RDSFeed supports three data sources:

* Media files
* Facebook
* Foursquare

Each data source is controlled through configuration.

### Media files

```
    <Source name="LORMediaFile" type="GreenlakeChristmas.RDSFeed.DataSources.MediaFile.MediaFileDataSource">
      <RefreshInterval>5</RefreshInterval>
      <Priority>Immediate</Priority>
      <Constructor>
        <Parameter name="mediafilepath" value="c:\Users\me\song.txt" />
      </Constructor>
      <Templates>
        <Case when="" method="GetSongTitle" template="{0}" />
      </Templates>
    </Source>
```

This is the example configuration section for a media file data source. To understand operation, it's best
to look at each part of the configuration.

###### &lt;Source&gt; node
```
<Source name="LORMediaFile" type="GreenlakeChristmas.RDSFeed.DataSources.MediaFile.MediaFileDataSource">
```
The Source node defines a name and the programmatic type used for the data source.

@name - a descriptive name for the data source. The name must be unique in the list of all data sources in the 
rds.config file. If there are duplicates, RDSFeed will exit with an error.

@type - the programmatic type. This is the fully-qualified name of the class that supports this data source.
RDSFeed uses this information to locate the code used to execute the data source. This permits RDSFeed to be
extended by allowing other types of data sources to be created and added to RDSFeed.

###### &lt;RefreshInterval&gt; node
```
<RefreshInterval>5</RefreshInterval>
```
Refresh interval defines the "loop" speed for the data source, measured in seconds. A data source runs 
continuously, and is queried over and over by the RDSFeed application for new text. The refresh interval
tells RDSFeed how often it should make that query. In this example, this data source is asked for new 
text every 5 seconds.

###### &lt;Priority&gt; node
```
<Priority>Immediate</Priority>
```

Priority defines the importance of text updates from the data source. There are two permitted values for
Priority: General and Immediate. If the priority for a data source is "General", messages are updated on a
first-in-first-out basis. If the priority is "Immediate", text updates from the data source are immediately
pushed out. Due to operation, it's best that only one data source in an application be configured as Immediate.

