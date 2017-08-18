namespace Microsoft.Internal.Performance
{
    using System;

    internal enum CodeMarkerEvent
    {
        perfVSWhitehorseBackgroundSyncEnd = 0x202d,
        perfVSWhitehorseCodeHandlerCodeModelEventBegin = 0x201f,
        perfVSWhitehorseCodeHandlerCodeModelEventEnd = 0x2020,
        perfVSWhitehorseCodeHandlerOnChangeLineTextBegin = 0x201d,
        perfVSWhitehorseCodeHandlerOnChangeLineTextEnd = 0x201e,
        perfVSWhitehorseCodeHandlerWalkFileBegin = 0x201b,
        perfVSWhitehorseCodeHandlerWalkFileEnd = 0x201c,
        perfVSWhitehorseContextMenuBegin = 0x2021,
        perfVSWhitehorseContextMenuEnd = 0x2022,
        perfVSWhitehorseDropBegin = 0x200a,
        perfVSWhitehorseDropEnd = 0x200b,
        perfVSWhitehorseKeyDownBegin = 0x2010,
        perfVSWhitehorseKeyDownEnd = 0x2011,
        perfVSWhitehorseKeyUpBegin = 0x2012,
        perfVSWhitehorseKeyUpEnd = 0x2013,
        perfVSWhitehorseLoadDocumentBegin = 0x200e,
        perfVSWhitehorseLoadDocumentEnd = 0x200f,
        perfVSWhitehorseOperationDesignerExpandEnd = 0x201a,
        perfVSWhitehorseOperationDesignerExpandStart = 0x2019,
        perfVSWhitehorseOperationDesignerLoadEnd = 0x2018,
        perfVSWhitehorseOperationDesignerLoadStart = 0x2017,
        perfVSWhitehorsePaintBegin = 0x200c,
        perfVSWhitehorsePaintEnd = 0x200d,
        perfVSWhitehorseSCESearchBegin = 0x202e,
        perfVSWhitehorseSCESearchEnd = 0x202f,
        perfVSWhitehorseStartDragBegin = 0x2008,
        perfVSWhitehorseStartDragEnd = 0x2009,
        perfVSWhitehorseSyncBegin = 0x202b,
        perfVSWhitehorseSyncEnd = 0x202c,
        perfVSWhitehorseT4CodeGenerationBegin = 0x2030,
        perfVSWhitehorseT4CodeGenerationEnd = 0x2031,
        perfVSWhitehorseTopLevelTransactionBegin = 0x2014,
        perfVSWhitehorseTopLevelTransactionCommitEnd = 0x2016,
        perfVSWhitehorseTopLevelTransactionCommitStart = 0x2015
    }
}

