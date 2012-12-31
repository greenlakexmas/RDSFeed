# Foursquare

## Overview

The Foursquare data source uses the Fourswuare API. Access to Foursquare is facilitated through a dependency
on a [Foursquare wrapper library](https://github.com/greenlakexmas/FourSquare), also created by 
Greenlake Christmas.

Usage of the Foursquare API requires developer setup and configuration for a Foursquare location. The
[Foursquare Developer portal](https://developer.foursquare.com/) contains information for getting
established to use the Foursquare API.

## Configuration
```
    <Source name="FourSquare" type="GreenlakeChristmas.RDSFeed.DataSources.Foursquare.FoursquareDataSource">
      <RefreshInterval>30</RefreshInterval>
      <Priority>General</Priority>
      <Constructor>
        <Parameter name="oauthtoken" value="{your-oauth-token}" />
        <Parameter name="venueid" value="{your-venue-id}" />
        <Parameter name="datafilepath" value="c:\Users\me\4sq.txt" />
      </Constructor>
      <Templates rotation="RoundRobin">
        <Case when="Checkin" method="GetCheckin" template="{0} checked in on 4square on {1} {2}" />
        <Case when="Mayor" method="GetMayor" template="{0} is the Mayor of Greenlake Christmas on 4square" />
      </Templates>
    </Source>
```

## Constructor parameters

* oauthtoken - the OAuth token, provided by Foursquare.
* venueid - the venue Id, provided by Foursquare.
* datafilepath - a data file path on the local file system.

## Template methods

[GetCheckin](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Foursquare/FoursquareDataSource.cs#L31)

#### Returns

String array, index 0: the name of a person who checked in on Foursquare.
              index 1: the month of the check-in.
              index 2: the day of the check-in, in text-ordinal form, i.e. the "20th".

---

[GetMayor](https://github.com/greenlakexmas/RDSFeed/blob/master/DataSources/Foursquare/FoursquareDataSource.cs#L52)

#### Returns

String array, index 0: the name of the person recognized as "Mayor" of this Foursquare venue Id.

