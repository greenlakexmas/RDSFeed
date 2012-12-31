# Text Files

## Overview

A text file data source monitors a separate text file for changes and writes the content
of the text file out as a text message. The text file is assumed to contain one line of text.

## Configuration
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

## Constructor parameters

* textfilepath - path to a source text file to be monitored.

## Template methods

[GetSongTitle](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/TextFile/TextFileDataSource.cs#L17)

#### Returns

String array, index 0: the first line of the monitored text file

* If the source text file exists, the file content is read (the first line)
* If the content has changed from previous, a string array of length 1 with the first line of the text file
