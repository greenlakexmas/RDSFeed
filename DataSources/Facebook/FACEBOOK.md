# Facebook

## Overview

The Facebook data source uses the Facebook API. Access to Facebook is facilitated through a dependency
on the [C# Facebook SDK](http://csharpsdk.org/).

Usage of the Facebook API requires developer setup and configuration for a Facebook page/location. The 
[Facebook Developer portal](https://developers.facebook.com/) contains information for getting 
established to use the Facebook API.

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
* objectid - the object id, provided by Facebook. Object Id is presumed to be a Facebook page with location.

## Template methods

[GetCheckins](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Facebook/FacebookDataSource.cs#L87)

#### Returns

String array, index 0: the number of people who have checked in at the Facebook place.

---

[GetTalkingAboutCount](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Facebook/FacebookDataSource.cs#L94)

#### Returns

String array, index 0: the number of people who have shared posts and/or are "talking about" this Facebook object Id.

---

[GetLikes](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Facebook/FacebookDataSource.cs#L101)

#### Returns

String array, index 0: the number of people who have "liked" this Facebook object Id.

---

[GetWereHereCount](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Facebook/FacebookDataSource.cs#L108)

#### Returns

String array, index 0: the number of people who "were here" at this Facebook object Id.

