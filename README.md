RDSFeed
=======

RDSFeed is an application for constructing timely text statements from multiple inforation sources for use over RDS. 
Actually, "RDS" (which standard for Radio Data System) is a misnomer here -- I use this application for sending 
data over RDS. However, it can just as easily work for other systems that can accept text, such as scrolling 
LED readers.


What it does
============

RDSFeed is an operational container for producing text statements in a timely manner. The RDSFeed application
produces a text file as its output, containing a text string. The application continually updates this text file, 
replacing the content of the file with new text statements. What those statements contain are completely 
configurable through template configuration. This type of operation is useful in applications such as RDS.


The output of RDSFeed is a text file. This text file will contain a statement of some sort, which is controlled
through configuration files. When the RDSFeed application runs, an output text file is produced and continually 
updated by the application (as long as it is running.) In graphical form:

----------------
|              |
|     RDS      |         ==== >>>       output.txt
|     Feed     |
|              |
----------------

Many systems that can accept text string input, such as FM transmitters that support RDS or scrolling LED devices, 
will monitor a text file that serves as the input source. RDSFeed is constructed to create such an output file.


How it works
============

RDSFeed is both configurable and extensible.

Configuration
-------------

