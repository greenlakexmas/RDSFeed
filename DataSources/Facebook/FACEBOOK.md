# Facebook

## Overview

The Facebook data source uses the Facebook API. Access to Facebook is facilitated through a dependency
on the [C# Facebook SDK](http://csharpsdk.org/).

## Configuration
```
    <Source name="Facebook" type="GreenlakeChristmas.RDSFeed.DataSources.Facebook.FacebookDataSource">
      <RefreshInterval>30</RefreshInterval>
      <Priority>General</Priority>
      <Constructor>
        <Parameter name="applicationid" value="{your-application-id}" />
        <Parameter name="applicationsecret" value="{your-application-secret}" />
        <Parameter name="objectid" value="{your-objectid}" />
      </Constructor>
      <Templates rotation="RoundRobin">
        <Case when="Checkins" method="GetCheckins" template="{0} people have checked in at Greenlake Christmas on Facebook" />
        <Case when="TalkingAboutCount" method="GetTalkingAboutCount" template="{0} people are talking about Greenlake Christmas on Facebook" />
        <Case when="Likes" method="GetLikes" template="{0} people like Greenlake Christmas on Facebook" />
        <Case when="WereHereCount" method="GetWereHereCount" template="{0} people were at Greenlake Christmas on Facebook" />
      </Templates>
    </Source>
```

## Constructor parameters

* applicationid - the application id, provided by Facebook.
* applicationsecret - the application secret, provided by Facebook.
* objectid - the object id, provided by Facebook.

## Template methods

[GetCheckins](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/TextFile/TextFileDataSource.cs#L17)

#### Returns

String array, index 0: the first line of the monitored text file

* If the source text file exists, the file content is read (the first line)
* If the content has changed from previous, a string array of length 1 with the first line of the text file
