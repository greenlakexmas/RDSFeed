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

* Text files
* Facebook
* Foursquare
* Twitter

Each data source is controlled through configuration.

### Text files

```
    <Source name="LORMediaFile" type="GreenlakeChristmas.RDSFeed.DataSources.TextFile.TextFileDataSource">
      <RefreshInterval>5</RefreshInterval>
      <Priority>Immediate</Priority>
      <Constructor>
        <Parameter name="textfilepath" value="c:\Users\me\song.txt" />
      </Constructor>
      <Templates>
        <Case when="SongTitle" method="GetSongTitle" template="{0}" />
      </Templates>
    </Source>
```

This is the example configuration section for a text file data source. To understand operation, it's best
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


###### &lt;Constructor&gt; node
```
<Constructor>
  <Parameter name="mediafilepath" value="c:\Users\me\song.txt" />
</Constructor>
```

The Constructor node defines the way the Source type is instanatiated or started. A Constructor contains a 
set of Parameter nodes, which represent the necessary information for the Source to operate. Parameters are
order and data-type sensitive. In this example, the Source Constructor takes a Parameter called "mediafilepath" 
and uses the value "c:\Users\me\song.txt".

The Parameters necessary for a Constructor are a function of documentation. Here are the parameter values
needed for each type of supported data source:

- MediaFileDataSource
```
<Constructor>
  <Parameter name="mediafilepath" value="c:\Users\me\song.txt" />
</Constructor>

```
- FoursquareDataSource
```
      <Constructor>
        <Parameter name="oauthtoken" value="ZYXWVUTSRQPONMLKJIHGFEDCBA987654321" />
        <Parameter name="venueid" value="a1b2c3d4e5f6g7h8i9" />
        <Parameter name="datafilepath" value="c:\Users\me\4sq.txt" />
      </Constructor>
```
- FacebookDataSource
```
      <Constructor>
        <Parameter name="applicationid" value="1234567890" />
        <Parameter name="applicationsecret" value="1234abcd5678efgh9012ijkl3456mnop" />
        <Parameter name="objectid" value="9876543210" />
      </Constructor>
```

###### &lt;Templates&gt; node
```
      <Templates>
        <Case when="" method="GetSongTitle" template="The song is {0}" />
      </Templates>
```

Templates define the different possible text message strings that a data source
can generate. Each template requires a programmatic method on the data source type
that can be executed on its behalf.

Each method returns a string array of varying length, which will be applied to
the template attribute to produce the text message. In this example, the data source
will execute the method GetSongTitle, which will produce data that's applied to the
template string "The song is {0}". GetSongTitle returns only a single value, which gets
substituted for {0}, creating the text string "The song is my-song-name-here".

Each data source can produce more than one text statement, which is accomplished through
pairs of methods and accompanying template strings against which to substitute values.

Pairing template strings to methods requires knowledge of the programmatic method. Each
method provided must be documented to describe what's permitted within template text.
