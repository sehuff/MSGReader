﻿//
// MimeType.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2013-2024 Kees van Spelde. (www.magic-sessions.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

// ReSharper disable All

using System.Collections.Generic;
using System.IO;
using System;

namespace MsgReader.Helpers
{
    /// <summary>
    ///     This class contains all known mimetypes
    /// </summary>
    internal static class MimeTypes
    {
        #region Fields
        private static readonly Dictionary<string, string> extensions;
        private static readonly Dictionary<string, string> mimeTypes;
        #endregion

        #region Constructor
        static MimeTypes()
        {
            extensions = LoadExtensions();
            mimeTypes = LoadMimeTypes();
        }
        #endregion

        #region LoadMimeTypes
        internal static Dictionary<string, string> LoadMimeTypes()
        {
            return new Dictionary<string, string>()
            {
                { ".323", "text/h323" },
                { ".3g2", "video/3gpp2" },
                { ".3gp", "video/3gpp" },
                { ".7z", "application/x-7z-compressed" },
                { ".aab", "application/x-authorware-bin" },
                { ".aac", "audio/aac" },
                { ".aam", "application/x-authorware-map" },
                { ".aas", "application/x-authorware-seg" },
                { ".abc", "text/vnd.abc" },
                { ".acgi", "text/html" },
                { ".acx", "application/internet-property-stream" },
                { ".afl", "video/animaflex" },
                { ".ai", "application/postscript" },
                { ".aif", "audio/aiff" },
                { ".aifc", "audio/aiff" },
                { ".aiff", "audio/aiff" },
                { ".aim", "application/x-aim" },
                { ".aip", "text/x-audiosoft-intra" },
                { ".ani", "application/x-navi-animation" },
                { ".aos", "application/x-nokia-9000-communicator-add-on-software" },
                { ".appcache", "text/cache-manifest" },
                { ".application", "application/x-ms-application" },
                { ".aps", "application/mime" },
                { ".art", "image/x-jg" },
                { ".asf", "video/x-ms-asf" },
                { ".asm", "text/x-asm" },
                { ".asp", "text/asp" },
                { ".asr", "video/x-ms-asf" },
                { ".asx", "application/x-mplayer2" },
                { ".atom", "application/atom+xml" },
                { ".au", "audio/x-au" },
                { ".avi", "video/avi" },
                { ".avs", "video/avs-video" },
                { ".axs", "application/olescript" },
                { ".azw", "application/vnd.amazon.ebook" },
                { ".bas", "text/plain" },
                { ".bcpio", "application/x-bcpio" },
                { ".bin", "application/octet-stream" },
                { ".bm", "image/bmp" },
                { ".bmp", "image/bmp" },
                { ".boo", "application/book" },
                { ".book", "application/book" },
                { ".boz", "application/x-bzip2" },
                { ".bsh", "application/x-bsh" },
                { ".bz2", "application/x-bzip2" },
                { ".bz", "application/x-bzip" },
                { ".cat", "application/vnd.ms-pki.seccat" },
                { ".ccad", "application/clariscad" },
                { ".cco", "application/x-cocoa" },
                { ".cc", "text/plain" },
                { ".cdf", "application/cdf" },
                { ".cer", "application/pkix-cert" },
                { ".cha", "application/x-chat" },
                { ".chat", "application/x-chat" },
                { ".chm", "application/vnd.ms-htmlhelp" },
                { ".class", "application/x-java-applet" },
                { ".clp", "application/x-msclip" },
                { ".cmx", "image/x-cmx" },
                { ".cod", "image/cis-cod" },
                { ".coffee", "text/x-coffeescript" },
                { ".conf", "text/plain" },
                { ".cpio", "application/x-cpio" },
                { ".cpp", "text/plain" },
                { ".cpt", "application/x-cpt" },
                { ".crd", "application/x-mscardfile" },
                { ".crl", "application/pkix-crl" },
                { ".crt", "application/pkix-cert" },
                { ".csh", "application/x-csh" },
                { ".css", "text/css" },
                { ".csv", "text/csv" },
                { ".cs", "text/plain" },
                { ".c", "text/plain" },
                { ".c++", "text/plain" },
                { ".cxx", "text/plain" },
                { ".dart", "application/dart" },
                { ".dcr", "application/x-director" },
                { ".deb", "application/x-deb" },
                { ".deepv", "application/x-deepv" },
                { ".def", "text/plain" },
                { ".deploy", "application/octet-stream" },
                { ".der", "application/x-x509-ca-cert" },
                { ".dib", "image/bmp" },
                { ".dif", "video/x-dv" },
                { ".dir", "application/x-director" },
                { ".disco", "text/xml" },
                { ".dll", "application/x-msdownload" },
                { ".dl", "video/dl" },
                { ".doc", "application/msword" },
                { ".docm", "application/vnd.ms-word.document.macroEnabled.12" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".dot", "application/msword" },
                { ".dotm", "application/vnd.ms-word.template.macroEnabled.12" },
                { ".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
                { ".dp", "application/commonground" },
                { ".drw", "application/drafting" },
                { ".dtd", "application/xml-dtd" },
                { ".dvi", "application/x-dvi" },
                { ".dv", "video/x-dv" },
                { ".dwg", "application/acad" },
                { ".dxf", "application/dxf" },
                { ".dxr", "application/x-director" },
                { ".el", "text/x-script.elisp" },
                { ".elc", "application/x-elc" },
                { ".emf", "image/emf" },
                { ".eml", "message/rfc822" },
                { ".eot", "application/vnd.bw-fontobject" },
                { ".eps", "application/postscript" },
                { ".epub", "application/epub+zip" },
                { ".es", "application/x-esrehber" },
                { ".etx", "text/x-setext" },
                { ".evy", "application/envoy" },
                { ".exe", "application/octet-stream" },
                { ".f77", "text/plain" },
                { ".f90", "text/plain" },
                { ".fdf", "application/vnd.fdf" },
                { ".fif", "image/fif" },
                { ".flac", "audio/x-flac" },
                { ".fli", "video/fli" },
                { ".flx", "text/vnd.fmi.flexstor" },
                { ".fmf", "video/x-atomic3d-feature" },
                { ".for", "text/plain" },
                { ".fpx", "image/vnd.fpx" },
                { ".frl", "application/freeloader" },
                { ".fsx", "application/fsharp-script" },
                { ".g3", "image/g3fax" },
                { ".gif", "image/gif" },
                { ".gl", "video/gl" },
                { ".gsd", "audio/x-gsm" },
                { ".gsm", "audio/x-gsm" },
                { ".gsp", "application/x-gsp" },
                { ".gss", "application/x-gss" },
                { ".gtar", "application/x-gtar" },
                { ".g", "text/plain" },
                { ".gz", "application/x-gzip" },
                { ".gzip", "application/x-gzip" },
                { ".hdf", "application/x-hdf" },
                { ".help", "application/x-helpfile" },
                { ".hgl", "application/vnd.hp-HPGL" },
                { ".hh", "text/plain" },
                { ".hlb", "text/x-script" },
                { ".hlp", "application/x-helpfile" },
                { ".hpg", "application/vnd.hp-HPGL" },
                { ".hpgl", "application/vnd.hp-HPGL" },
                { ".hqx", "application/binhex" },
                { ".hta", "application/hta" },
                { ".htc", "text/x-component" },
                { ".h", "text/plain" },
                { ".htmls", "text/html" },
                { ".html", "text/html" },
                { ".htm", "text/html" },
                { ".htt", "text/webviewhtml" },
                { ".htx", "text/html" },
                { ".ico", "image/vnd.microsoft.icon" },
                { ".ics", "text/calendar" },
                { ".idc", "text/plain" },
                { ".ief", "image/ief" },
                { ".iefs", "image/ief" },
                { ".iges", "model/iges" },
                { ".igs", "model/iges" },
                { ".iii", "application/x-iphone" },
                { ".ima", "application/x-ima" },
                { ".imap", "application/x-httpd-imap" },
                { ".inf", "application/inf" },
                { ".ins", "application/x-internett-signup" },
                { ".ip", "application/x-ip2" },
                { ".isp", "application/x-internet-signup" },
                { ".isu", "video/x-isvideo" },
                { ".it", "audio/it" },
                { ".iv", "application/x-inventor" },
                { ".ivf", "video/x-ivf" },
                { ".ivy", "application/x-livescreen" },
                { ".jam", "audio/x-jam" },
                { ".jar", "application/java-archive" },
                { ".java", "text/plain" },
                { ".jav", "text/plain" },
                { ".jcm", "application/x-java-commerce" },
                { ".jfif", "image/jpeg" },
                { ".jfif-tbnl", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".jpe", "image/jpeg" },
                { ".jpg", "image/jpeg" },
                { ".jps", "image/x-jps" },
                { ".js", "text/javascript" },
                { ".json", "application/json" },
                { ".jsonld", "application/ld+json" },
                { ".jut", "image/jutvision" },
                { ".kar", "audio/midi" },
                { ".ksh", "text/x-script.ksh" },
                { ".la", "audio/nspaudio" },
                { ".lam", "audio/x-liveaudio" },
                { ".latex", "application/x-latex" },
                { ".list", "text/plain" },
                { ".lma", "audio/nspaudio" },
                { ".log", "text/plain" },
                { ".lsp", "application/x-lisp" },
                { ".lst", "text/plain" },
                { ".lsx", "text/x-la-asf" },
                { ".ltx", "application/x-latex" },
                { ".m13", "application/x-msmediaview" },
                { ".m14", "application/x-msmediaview" },
                { ".m1v", "video/mpeg" },
                { ".m2a", "audio/mpeg" },
                { ".m2v", "video/mpeg" },
                { ".m3u", "audio/x-mpequrl" },
                { ".m4a", "audio/mp4" },
                { ".m4v", "video/mp4" },
                { ".man", "application/x-troff-man" },
                { ".manifest", "application/x-ms-manifest" },
                { ".map", "application/x-navimap" },
                { ".mar", "text/plain" },
                { ".markdown", "text/markdown" },
                { ".mbd", "application/mbedlet" },
                { ".mc$", "application/x-magic-cap-package-1.0" },
                { ".mcd", "application/mcad" },
                { ".mcf", "image/vasa" },
                { ".mcp", "application/netmc" },
                { ".md", "text/markdown" },
                { ".mdb", "application/x-msaccess" },
                { ".mesh", "model/mesh" },
                { ".me", "application/x-troff-me" },
                { ".mid", "audio/midi" },
                { ".midi", "audio/midi" },
                { ".mif", "application/x-mif" },
                { ".mjf", "audio/x-vnd.AudioExplosion.MjuiceMediaFile" },
                { ".mjpg", "video/x-motion-jpeg" },
                { ".mjs", "text/javascript" },
                { ".mm", "application/base64" },
                { ".mme", "application/base64" },
                { ".mny", "application/x-msmoney" },
                { ".mod", "audio/mod" },
                { ".mov", "video/quicktime" },
                { ".movie", "video/x-sgi-movie" },
                { ".mp2", "video/mpeg" },
                { ".mp3", "audio/mpeg" },
                { ".mp4", "video/mp4" },
                { ".mp4a", "audio/mp4" },
                { ".mp4v", "video/mp4" },
                { ".mpa", "audio/mpeg" },
                { ".mpc", "application/x-project" },
                { ".mpeg", "video/mpeg" },
                { ".mpe", "video/mpeg" },
                { ".mpga", "audio/mpeg" },
                { ".mpg", "video/mpeg" },
                { ".mpp", "application/vnd.ms-project" },
                { ".mpt", "application/x-project" },
                { ".mpv2", "video/mpeg" },
                { ".mpv", "application/x-project" },
                { ".mpx", "application/x-project" },
                { ".mrc", "application/marc" },
                { ".ms", "application/x-troff-ms" },
                { ".msg", "application/vnd.ms-outlook" },
                { ".msh", "model/mesh" },
                { ".m", "text/plain" },
                { ".mvb", "application/x-msmediaview" },
                { ".mv", "video/x-sgi-movie" },
                { ".mzz", "application/x-vnd.AudioExplosion.mzz" },
                { ".nap", "image/naplps" },
                { ".naplps", "image/naplps" },
                { ".nc", "application/x-netcdf" },
                { ".ncm", "application/vnd.nokia.configuration-message" },
                { ".niff", "image/x-niff" },
                { ".nif", "image/x-niff" },
                { ".nix", "application/x-mix-transfer" },
                { ".nsc", "application/x-conference" },
                { ".nvd", "application/x-navidoc" },
                { ".nws", "message/rfc822" },
                { ".oda", "application/oda" },
                { ".odp", "application/vnd.oasis.opendocument.presentation" },
                { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
                { ".odt", "application/vnd.oasis.opendocument.text" },
                { ".oga", "audio/ogg" },
                { ".ogg", "audio/ogg" },
                { ".ogv", "video/ogg" },
                { ".ogx", "application/ogg" },
                { ".omc", "application/x-omc" },
                { ".omcd", "application/x-omcdatamaker" },
                { ".omcr", "application/x-omcregerator" },
                { ".opus", "audio/opus" },
                { ".otf", "font/otf" },
                { ".oxps", "application/oxps" },
                { ".p10", "application/pkcs10" },
                { ".p12", "application/pkcs-12" },
                { ".p7a", "application/x-pkcs7-signature" },
                { ".p7b", "application/x-pkcs7-certificates" },
                { ".p7c", "application/pkcs7-mime" },
                { ".p7m", "application/pkcs7-mime" },
                { ".p7r", "application/x-pkcs7-certreqresp" },
                { ".p7s", "application/pkcs7-signature" },
                { ".part", "application/pro_eng" },
                { ".pas", "text/pascal" },
                { ".pbm", "image/x-portable-bitmap" },
                { ".pcl", "application/x-pcl" },
                { ".pct", "image/x-pict" },
                { ".pcx", "image/x-pcx" },
                { ".pdf", "application/pdf" },
                { ".pfx", "application/x-pkcs12" },
                { ".pgm", "image/x-portable-graymap" },
                { ".pic", "image/pict" },
                { ".pict", "image/pict" },
                { ".pkg", "application/x-newton-compatible-pkg" },
                { ".pko", "application/vnd.ms-pki.pko" },
                { ".pl", "text/plain" },
                { ".plx", "application/x-PiXCLscript" },
                { ".pm4", "application/x-pagemaker" },
                { ".pm5", "application/x-pagemaker" },
                { ".pma", "application/x-perfmon" },
                { ".pmc", "application/x-perfmon" },
                { ".pm", "image/x-xpixmap" },
                { ".pml", "application/x-perfmon" },
                { ".pmr", "application/x-perfmon" },
                { ".pmw", "application/x-perfmon" },
                { ".png", "image/png" },
                { ".pnm", "application/x-portable-anymap" },
                { ".pot", "application/vnd.ms-powerpoint" },
                { ".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12" },
                { ".potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
                { ".pov", "model/x-pov" },
                { ".ppa", "application/vnd.ms-powerpoint" },
                { ".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12" },
                { ".ppm", "image/x-portable-pixmap" },
                { ".pps", "application/vnd.ms-powerpoint" },
                { ".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" },
                { ".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
                { ".ppt", "application/vnd.ms-powerpoint" },
                { ".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12" },
                { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { ".ppz", "application/mspowerpoint" },
                { ".pre", "application/x-freelance" },
                { ".prf", "application/pics-rules" },
                { ".prt", "application/pro_eng" },
                { ".ps", "application/postscript" },
                { ".p", "text/x-pascal" },
                { ".pub", "application/x-mspublisher" },
                { ".pwz", "application/vnd.ms-powerpoint" },
                { ".pyc", "application/x-bytecode.python" },
                { ".py", "text/x-script.phyton" },
                { ".qcp", "audio/vnd.qcelp" },
                { ".qif", "image/x-quicktime" },
                { ".qtc", "video/x-qtc" },
                { ".qtif", "image/x-quicktime" },
                { ".qti", "image/x-quicktime" },
                { ".qt", "video/quicktime" },
                { ".ra", "audio/x-pn-realaudio" },
                { ".ram", "audio/x-pn-realaudio" },
                { ".rar", "application/vnd.rar" },
                { ".ras", "application/x-cmu-raster" },
                { ".rast", "image/cmu-raster" },
                { ".rexx", "text/x-script.rexx" },
                { ".rf", "image/vnd.rn-realflash" },
                { ".rgb", "image/x-rgb" },
                { ".rm", "application/vnd.rn-realmedia" },
                { ".rmi", "audio/mid" },
                { ".rmm", "audio/x-pn-realaudio" },
                { ".rmp", "audio/x-pn-realaudio" },
                { ".rng", "application/ringing-tones" },
                { ".rnx", "application/vnd.rn-realplayer" },
                { ".roff", "application/x-troff" },
                { ".rp", "image/vnd.rn-realpix" },
                { ".rpm", "audio/x-pn-realaudio-plugin" },
                { ".rss", "application/rss+xml" },
                { ".rtf", "text/rtf" },
                { ".rt", "text/richtext" },
                { ".rtx", "text/richtext" },
                { ".rv", "video/vnd.rn-realvideo" },
                { ".s3m", "audio/s3m" },
                { ".sbk", "application/x-tbook" },
                { ".scd", "application/x-msschedule" },
                { ".scm", "application/x-lotusscreencam" },
                { ".sct", "text/scriptlet" },
                { ".sdml", "text/plain" },
                { ".sdp", "application/sdp" },
                { ".sdr", "application/sounder" },
                { ".sea", "application/sea" },
                { ".set", "application/set" },
                { ".setpay", "application/set-payment-initiation" },
                { ".setreg", "application/set-registration-initiation" },
                { ".sgml", "text/sgml" },
                { ".sgm", "text/sgml" },
                { ".shar", "application/x-bsh" },
                { ".sh", "text/x-script.sh" },
                { ".shtml", "text/html" },
                { ".sid", "audio/x-psid" },
                { ".silo", "model/mesh" },
                { ".sit", "application/x-sit" },
                { ".skd", "application/x-koan" },
                { ".skm", "application/x-koan" },
                { ".skp", "application/x-koan" },
                { ".skt", "application/x-koan" },
                { ".sl", "application/x-seelogo" },
                { ".smi", "application/smil" },
                { ".smil", "application/smil" },
                { ".snd", "audio/basic" },
                { ".sol", "application/solids" },
                { ".spc", "application/x-pkcs7-certificates" },
                { ".spl", "application/futuresplash" },
                { ".spr", "application/x-sprite" },
                { ".sprite", "application/x-sprite" },
                { ".spx", "audio/ogg" },
                { ".src", "application/x-wais-source" },
                { ".ssi", "text/x-server-parsed-html" },
                { ".ssm", "application/streamingmedia" },
                { ".sst", "application/vnd.ms-pki.certstore" },
                { ".step", "application/step" },
                { ".s", "text/x-asm" },
                { ".stl", "application/sla" },
                { ".stm", "text/html" },
                { ".stp", "application/step" },
                { ".sv4cpio", "application/x-sv4cpio" },
                { ".sv4crc", "application/x-sv4crc" },
                { ".svf", "image/x-dwg" },
                { ".svg", "image/svg+xml" },
                { ".svr", "application/x-world" },
                { ".swf", "application/x-shockwave-flash" },
                { ".talk", "text/x-speech" },
                { ".t", "application/x-troff" },
                { ".tar", "application/x-tar" },
                { ".tbk", "application/toolbook" },
                { ".tcl", "text/x-script.tcl" },
                { ".tcsh", "text/x-script.tcsh" },
                { ".tex", "application/x-tex" },
                { ".texi", "application/x-texinfo" },
                { ".texinfo", "application/x-texinfo" },
                { ".text", "text/plain" },
                { ".tgz", "application/x-compressed" },
                { ".tiff", "image/tiff" },
                { ".tif", "image/tiff" },
                { ".tr", "application/x-troff" },
                { ".trm", "application/x-msterminal" },
                { ".ts", "application/typescript" },
                { ".tsi", "audio/tsp-audio" },
                { ".tsp", "audio/tsplayer" },
                { ".tsv", "text/tab-separated-values" },
                { ".ttc", "font/collection" },
                { ".ttf", "font/ttf" },
                { ".txt", "text/plain" },
                { ".uil", "text/x-uil" },
                { ".uls", "text/iuls" },
                { ".unis", "text/uri-list" },
                { ".uni", "text/uri-list" },
                { ".unv", "application/i-deas" },
                { ".uris", "text/uri-list" },
                { ".uri", "text/uri-list" },
                { ".ustar", "multipart/x-ustar" },
                { ".uue", "text/x-uuencode" },
                { ".uu", "text/x-uuencode" },
                { ".vcd", "application/x-cdlink" },
                { ".vcf", "text/vcard" },
                { ".vcard", "text/vcard" },
                { ".vcs", "text/x-vcalendar" },
                { ".vda", "application/vda" },
                { ".vdo", "video/vdo" },
                { ".vew", "application/groupwise" },
                { ".vivo", "video/vnd.vivo" },
                { ".viv", "video/vnd.vivo" },
                { ".vmd", "application/vocaltec-media-desc" },
                { ".vmf", "application/vocaltec-media-file" },
                { ".voc", "audio/voc" },
                { ".vos", "video/vosaic" },
                { ".vox", "audio/voxware" },
                { ".vqe", "audio/x-twinvq-plugin" },
                { ".vqf", "audio/x-twinvq" },
                { ".vql", "audio/x-twinvq-plugin" },
                { ".vrml", "application/x-vrml" },
                { ".vsd", "application/x-visio" },
                { ".vst", "application/x-visio" },
                { ".vsw", "application/x-visio" },
                { ".w60", "application/wordperfect6.0" },
                { ".w61", "application/wordperfect6.1" },
                { ".w6w", "application/msword" },
                { ".wav", "audio/wav" },
                { ".wb1", "application/x-qpro" },
                { ".wbmp", "image/vnd.wap.wbmp" },
                { ".wcm", "application/vnd.ms-works" },
                { ".wdb", "application/vnd.ms-works" },
                { ".web", "application/vnd.xara" },
                { ".weba", "audio/webm" },
                { ".webm", "video/webm" },
                { ".webp", "image/webp" },
                { ".wiz", "application/msword" },
                { ".wk1", "application/x-123" },
                { ".wks", "application/vnd.ms-works" },
                { ".wmf", "image/wmf" },
                { ".wmlc", "application/vnd.wap.wmlc" },
                { ".wmlsc", "application/vnd.wap.wmlscriptc" },
                { ".wmls", "text/vnd.wap.wmlscript" },
                { ".wml", "text/vnd.wap.wml" },
                { ".wmp", "video/x-ms-wmp" },
                { ".wmv", "video/x-ms-wmv" },
                { ".wmx", "video/x-ms-wmx" },
                { ".woff", "font/woff" },
                { ".woff2", "font/woff2" },
                { ".word", "application/msword" },
                { ".wp5", "application/wordperfect" },
                { ".wp6", "application/wordperfect" },
                { ".wp", "application/wordperfect" },
                { ".wpd", "application/wordperfect" },
                { ".wps", "application/vnd.ms-works" },
                { ".wq1", "application/x-lotus" },
                { ".wri", "application/mswrite" },
                { ".wrl", "application/x-world" },
                { ".wrz", "model/vrml" },
                { ".wsc", "text/scriplet" },
                { ".wsdl", "text/xml" },
                { ".wsrc", "application/x-wais-source" },
                { ".wtk", "application/x-wintalk" },
                { ".wvx", "video/x-ms-wvx" },
                { ".x3d", "model/x3d+xml" },
                { ".x3db", "model/x3d+fastinfoset" },
                { ".x3dv", "model/x3d-vrml" },
                { ".xaml", "application/xaml+xml" },
                { ".xap", "application/x-silverlight-app" },
                { ".xbap", "application/x-ms-xbap" },
                { ".xbm", "image/x-xbitmap" },
                { ".xdr", "video/x-amt-demorun" },
                { ".xht", "application/xhtml+xml" },
                { ".xhtml", "application/xhtml+xml" },
                { ".xif", "image/vnd.xiff" },
                { ".xla", "application/vnd.ms-excel" },
                { ".xlam", "application/vnd.ms-excel.addin.macroEnabled.12" },
                { ".xl", "application/excel" },
                { ".xlb", "application/excel" },
                { ".xlc", "application/excel" },
                { ".xld", "application/excel" },
                { ".xlk", "application/excel" },
                { ".xll", "application/excel" },
                { ".xlm", "application/excel" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12" },
                { ".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".xlt", "application/vnd.ms-excel" },
                { ".xltm", "application/vnd.ms-excel.template.macroEnabled.12" },
                { ".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
                { ".xlv", "application/excel" },
                { ".xlw", "application/excel" },
                { ".xm", "audio/xm" },
                { ".xml", "text/xml" },
                { ".xpi", "application/x-xpinstall" },
                { ".xpix", "application/x-vnd.ls-xpix" },
                { ".xpm", "image/xpm" },
                { ".xps", "application/vnd.ms-xpsdocument" },
                { ".x-png", "image/png" },
                { ".xsd", "text/xml" },
                { ".xsl", "text/xml" },
                { ".xslt", "text/xml" },
                { ".xsr", "video/x-amt-showrun" },
                { ".xwd", "image/x-xwd" },
                { ".z", "application/x-compressed" },
                { ".zip", "application/zip" },
                { ".zsh", "text/x-script.zsh" }
            };
        }
        #endregion

        #region LoadExtensions
        static Dictionary<string, string> LoadExtensions()
        {
            return new Dictionary<string, string>()
            {
                { "application/acad", ".dwg" },
                { "application/atom+xml", ".atom" },
                { "application/base64", ".mm" },
                { "application/binhex", ".hqx" },
                { "application/book", ".boo" },
                { "application/cdf", ".cdf" },
                { "application/clariscad", ".ccad" },
                { "application/commonground", ".dp" },
                { "application/dart", ".dart" },
                { "application/drafting", ".drw" },
                { "application/dxf", ".dxf" },
                { "application/envoy", ".evy" },
                { "application/epub+zip", ".epub" },
                { "application/excel", ".xls" },
                { "application/freeloader", ".frl" },
                { "application/fsharp-script", ".fsx" },
                { "application/futuresplash", ".spl" },
                { "application/groupwise", ".vew" },
                { "application/hta", ".hta" },
                { "application/i-deas", ".unv" },
                { "application/inf", ".inf" },
                { "application/internet-property-stream", ".acx" },
                { "application/java-archive", ".jar" },
                { "application/javascript", ".js" },
                { "application/json", ".json" },
                { "application/marc", ".mrc" },
                { "application/mbedlet", ".mbd" },
                { "application/mcad", ".mcd" },
                { "application/mime", ".aps" },
                { "application/mspowerpoint", ".ppz" },
                { "application/msword", ".doc" },
                { "application/mswrite", ".wri" },
                { "application/netmc", ".mcp" },
                { "application/octet-stream", ".bin" },
                { "application/oda", ".oda" },
                { "application/ogg", ".ogx" },
                { "application/oleobject", ".ods" },
                { "application/olescript", ".axs" },
                { "application/oxps", ".oxps" },
                { "application/pdf", ".pdf" },
                { "application/pics-rules", ".prf" },
                { "application/pkcs-12", ".p12" },
                { "application/pkcs10", ".p10" },
                { "application/pkcs7-mime", ".p7m" },
                { "application/pkcs7-signature", ".p7s" },
                { "application/pkix-cert", ".cer" },
                { "application/pkix-crl", ".crl" },
                { "application/postscript", ".ps" },
                { "application/pro_eng", ".part" },
                { "application/ringing-tones", ".rng" },
                { "application/rss+xml", ".rss" },
                { "application/sdp", ".sdp" },
                { "application/sea", ".sea" },
                { "application/set", ".set" },
                { "application/set-payment-initiation", ".setpay" },
                { "application/set-registration-initiation", ".setreg" },
                { "application/sla", ".stl" },
                { "application/smil", ".smi" },
                { "application/solids", ".sol" },
                { "application/sounder", ".sdr" },
                { "application/step", ".step" },
                { "application/streamingmedia", ".ssm" },
                { "application/toolbook", ".tbk" },
                { "application/typescript", ".ts" },
                { "application/vda", ".vda" },
                { "application/vnd.amazon.ebook", ".azw" },
                { "application/vnd.bw-fontobject", ".eot" },
                { "application/vnd.fdf", ".fdf" },
                { "application/vnd.hp-HPGL", ".hgl" },
                { "application/vnd.ms-excel", ".xls" },
                { "application/vnd.ms-excel.addin.macroEnabled.12", ".xlam" },
                { "application/vnd.ms-excel.sheet.binary.macroEnabled.12", ".xlsb" },
                { "application/vnd.ms-excel.sheet.macroEnabled.12", ".xlsm" },
                { "application/vnd.ms-excel.template.macroEnabled.12", ".xltm" },
                { "application/vnd.ms-htmlhelp", ".chm" },
                { "application/vnd.ms-outlook", ".msg" },
                { "application/vnd.ms-pki.certstore", ".sst" },
                { "application/vnd.ms-pki.pko", ".pko" },
                { "application/vnd.ms-pki.seccat", ".cat" },
                { "application/vnd.ms-powerpoint", ".ppt" },
                { "application/vnd.ms-powerpoint.addin.macroEnabled.12", ".ppam" },
                { "application/vnd.ms-powerpoint.presentation.macroEnabled.12", ".pptm" },
                { "application/vnd.ms-powerpoint.slideshow.macroEnabled.12", ".ppsm" },
                { "application/vnd.ms-powerpoint.template.macroEnabled.12", ".potm" },
                { "application/vnd.ms-project", ".mpp" },
                { "application/vnd.ms-word.document.macroEnabled.12", ".docm" },
                { "application/vnd.ms-word.template.macroEnabled.12", ".dotm" },
                { "application/vnd.ms-works", ".wcm" },
                { "application/vnd.ms-xpsdocument", ".xps" },
                { "application/vnd.nokia.configuration-message", ".ncm" },
                { "application/vnd.oasis.opendocument.presentation", ".odp" },
                { "application/vnd.oasis.opendocument.spreadsheet", ".ods" },
                { "application/vnd.oasis.opendocument.text", ".odt" },
                { "application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx" },
                { "application/vnd.openxmlformats-officedocument.presentationml.slideshow", ".ppsx" },
                { "application/vnd.openxmlformats-officedocument.presentationml.template", ".potx" },
                { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx" },
                { "application/vnd.openxmlformats-officedocument.spreadsheetml.template", ".xltx" },
                { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx" },
                { "application/vnd.openxmlformats-officedocument.wordprocessingml.template", ".dotx" },
                { "application/vnd.rar", ".rar" },
                { "application/vnd.rn-realmedia", ".rm" },
                { "application/vnd.rn-realplayer", ".rnx" },
                { "application/vnd.wap.wmlc", ".wmlc" },
                { "application/vnd.wap.wmlscriptc", ".wmlsc" },
                { "application/vnd.xara", ".web" },
                { "application/vocaltec-media-desc", ".vmd" },
                { "application/vocaltec-media-file", ".vmf" },
                { "application/wordperfect", ".wp5" },
                { "application/wordperfect6.0", ".w60" },
                { "application/wordperfect6.1", ".w61" },
                { "application/x-123", ".wk1" },
                { "application/x-7z-compressed", ".7z" },
                { "application/x-aim", ".aim" },
                { "application/x-authorware-bin", ".aab" },
                { "application/x-authorware-map", ".aam" },
                { "application/x-authorware-seg", ".aas" },
                { "application/x-bcpio", ".bcpio" },
                { "application/x-bsh", ".bsh" },
                { "application/x-bytecode.python", ".pyc" },
                { "application/x-bzip", ".bz" },
                { "application/x-bzip2", ".bz2" },
                { "application/x-cdlink", ".vcd" },
                { "application/x-chat", ".cha" },
                { "application/x-cmu-raster", ".ras" },
                { "application/x-cocoa", ".cco" },
                { "application/x-compressed", ".z" },
                { "application/x-conference", ".nsc" },
                { "application/x-cpio", ".cpio" },
                { "application/x-cpt", ".cpt" },
                { "application/x-csh", ".csh" },
                { "application/x-deb", ".deb" },
                { "application/x-deepv", ".deepv" },
                { "application/x-director", ".dcr" },
                { "application/x-dvi", ".dvi" },
                { "application/x-elc", ".elc" },
                { "application/x-esrehber", ".es" },
                { "application/x-font-ttf", ".ttf" },
                { "application/x-freelance", ".pre" },
                { "application/x-gsp", ".gsp" },
                { "application/x-gss", ".gss" },
                { "application/x-gtar", ".gtar" },
                { "application/x-gzip", ".gz" },
                { "application/x-hdf", ".hdf" },
                { "application/x-helpfile", ".help" },
                { "application/x-httpd-imap", ".imap" },
                { "application/x-ima", ".ima" },
                { "application/x-internet-signup", ".isp" },
                { "application/x-internett-signup", ".ins" },
                { "application/x-inventor", ".iv" },
                { "application/x-ip2", ".ip" },
                { "application/x-iphone", ".iii" },
                { "application/x-java-applet", ".class" },
                { "application/x-java-commerce", ".jcm" },
                { "application/x-koan", ".skd" },
                { "application/x-latex", ".latex" },
                { "application/x-lisp", ".lsp" },
                { "application/x-livescreen", ".ivy" },
                { "application/x-lotus", ".wq1" },
                { "application/x-lotusscreencam", ".scm" },
                { "application/x-magic-cap-package-1.0", ".mc$" },
                { "application/x-mif", ".mif" },
                { "application/x-mix-transfer", ".nix" },
                { "application/x-mplayer2", ".asx" },
                { "application/x-ms-application", ".application" },
                { "application/x-ms-manifest", ".manifest" },
                { "application/x-ms-xbap", ".xbap" },
                { "application/x-msaccess", ".mdb" },
                { "application/x-mscardfile", ".crd" },
                { "application/x-msclip", ".clp" },
                { "application/x-msdownload", ".dll" },
                { "application/x-msmediaview", ".m13" },
                { "application/x-msmoney", ".mny" },
                { "application/x-mspublisher", ".pub" },
                { "application/x-msschedule", ".scd" },
                { "application/x-msterminal", ".trm" },
                { "application/x-navi-animation", ".ani" },
                { "application/x-navidoc", ".nvd" },
                { "application/x-navimap", ".map" },
                { "application/x-netcdf", ".nc" },
                { "application/x-newton-compatible-pkg", ".pkg" },
                { "application/x-nokia-9000-communicator-add-on-software", ".aos" },
                { "application/x-omc", ".omc" },
                { "application/x-omcdatamaker", ".omcd" },
                { "application/x-omcregerator", ".omcr" },
                { "application/x-pagemaker", ".pm4" },
                { "application/x-pcl", ".pcl" },
                { "application/x-perfmon", ".pma" },
                { "application/x-PiXCLscript", ".plx" },
                { "application/x-pkcs12", ".pfx" },
                { "application/x-pkcs7-certificates", ".p7b" },
                { "application/x-pkcs7-certreqresp", ".p7r" },
                { "application/x-pkcs7-signature", ".p7a" },
                { "application/x-portable-anymap", ".pnm" },
                { "application/x-project", ".mpc" },
                { "application/x-qpro", ".wb1" },
                { "application/x-seelogo", ".sl" },
                { "application/x-shockwave-flash", ".swf" },
                { "application/x-silverlight-app", ".xap" },
                { "application/x-sit", ".sit" },
                { "application/x-sprite", ".spr" },
                { "application/x-sv4cpio", ".sv4cpio" },
                { "application/x-sv4crc", ".sv4crc" },
                { "application/x-tar", ".tar" },
                { "application/x-tbook", ".sbk" },
                { "application/x-tex", ".tex" },
                { "application/x-texinfo", ".texi" },
                { "application/x-troff", ".roff" },
                { "application/x-troff-man", ".man" },
                { "application/x-troff-me", ".me" },
                { "application/x-troff-ms", ".ms" },
                { "application/x-visio", ".vsd" },
                { "application/x-vnd.AudioExplosion.mzz", ".mzz" },
                { "application/x-vnd.ls-xpix", ".xpix" },
                { "application/x-vrml", ".vrml" },
                { "application/x-wais-source", ".src" },
                { "application/x-wintalk", ".wtk" },
                { "application/x-woff", ".woff" },
                { "application/x-world", ".svr" },
                { "application/x-x509-ca-cert", ".der" },
                { "application/x-xpinstall", ".xpi" },
                { "application/xaml+xml", ".xaml" },
                { "application/xhtml+xml", ".xhtml" },
                { "application/xml", ".xml" },
                { "application/xml-dtd", ".dtd" },
                { "application/zip", ".zip" },
                { "audio/aac", ".aac" },
                { "audio/aiff", ".aiff" },
                { "audio/basic", ".snd" },
                { "audio/it", ".it" },
                { "audio/mid", ".rmi" },
                { "audio/midi", ".mid" },
                { "audio/mod", ".mod" },
                { "audio/mp4", ".mp4" },
                { "audio/mpeg", ".mp3" },
                { "audio/nspaudio", ".la" },
                { "audio/ogg", ".ogg" },
                { "audio/opus", ".opus" },
                { "audio/s3m", ".s3m" },
                { "audio/tsp-audio", ".tsi" },
                { "audio/tsplayer", ".tsp" },
                { "audio/vnd.qcelp", ".qcp" },
                { "audio/voc", ".voc" },
                { "audio/vorbis", ".ogg" },
                { "audio/voxware", ".vox" },
                { "audio/wav", ".wav" },
                { "audio/webm", ".weba" },
                { "audio/x-au", ".au" },
                { "audio/x-flac", ".flac" },
                { "audio/x-gsm", ".gsd" },
                { "audio/x-jam", ".jam" },
                { "audio/x-liveaudio", ".lam" },
                { "audio/x-mpequrl", ".m3u" },
                { "audio/x-pn-realaudio", ".ra" },
                { "audio/x-pn-realaudio-plugin", ".rpm" },
                { "audio/x-psid", ".sid" },
                { "audio/x-twinvq", ".vqf" },
                { "audio/x-twinvq-plugin", ".vqe" },
                { "audio/x-vnd.AudioExplosion.MjuiceMediaFile", ".mjf" },
                { "audio/xm", ".xm" },
                { "font/collection", ".ttc" },
                { "font/otf", ".otf" },
                { "font/sfnt", ".ttf" },
                { "font/ttf", ".ttf" },
                { "font/woff", ".woff" },
                { "font/woff2", ".woff2" },
                { "image/bmp", ".bmp" },
                { "image/cis-cod", ".cod" },
                { "image/cmu-raster", ".rast" },
                { "image/emf", ".emf" },
                { "image/fif", ".fif" },
                { "image/g3fax", ".g3" },
                { "image/gif", ".gif" },
                { "image/ief", ".ief" },
                { "image/jpeg", ".jpg" },
                { "image/jutvision", ".jut" },
                { "image/naplps", ".nap" },
                { "image/pict", ".pic" },
                { "image/png", ".png" },
                { "image/svg+xml", ".svg" },
                { "image/tiff", ".tif" },
                { "image/vasa", ".mcf" },
                { "image/vnd.fpx", ".fpx" },
                { "image/vnd.microsoft.icon", ".ico" },
                { "image/vnd.rn-realflash", ".rf" },
                { "image/vnd.rn-realpix", ".rp" },
                { "image/vnd.wap.wbmp", ".wbmp" },
                { "image/vnd.xiff", ".xif" },
                { "image/webp", ".webp" },
                { "image/wmf", ".wmf" },
                { "image/x-cmx", ".cmx" },
                { "image/x-emf", ".emf" },
                { "image/x-dwg", ".svf" },
                { "image/x-jg", ".art" },
                { "image/x-jps", ".jps" },
                { "image/x-niff", ".niff" },
                { "image/x-pcx", ".pcx" },
                { "image/x-pict", ".pct" },
                { "image/x-png", ".png" },
                { "image/x-portable-bitmap", ".pbm" },
                { "image/x-portable-graymap", ".pgm" },
                { "image/x-portable-pixmap", ".ppm" },
                { "image/x-quicktime", ".qif" },
                { "image/x-rgb", ".rgb" },
                { "image/x-xbitmap", ".xbm" },
                { "image/x-xpixmap", ".pm" },
                { "image/x-xwd", ".xwd" },
                { "image/xpm", ".xpm" },
                { "message/rfc822", ".eml" },
                { "model/iges", ".iges" },
                { "model/mesh", ".mesh" },
                { "model/vrml", ".wrz" },
                { "model/x-pov", ".pov" },
                { "model/x3d+fastinfoset", ".x3db" },
                { "model/x3d+xml", ".x3d" },
                { "model/x3d-vrml", ".x3dv" },
                { "multipart/x-ustar", ".ustar" },
                { "text/asp", ".asp" },
                { "text/cache-manifest", ".appcache" },
                { "text/calendar", ".ics" },
                { "text/css", ".css" },
                { "text/csv", ".csv" },
                { "text/h323", ".323" },
                { "text/html", ".html" },
                { "text/iuls", ".uls" },
                { "text/javascript", ".js" },
                { "text/markdown", ".md" },
                { "text/pascal", ".pas" },
                { "text/plain", ".txt" },
                { "text/richtext", ".rtx" },
                { "text/rtf", ".rtf" },
                { "text/scriplet", ".wsc" },
                { "text/scriptlet", ".sct" },
                { "text/sgml", ".sgml" },
                { "text/tab-separated-values", ".tsv" },
                { "text/uri-list", ".uri" },
                { "text/vcard", ".vcf" },
                { "text/vnd.abc", ".abc" },
                { "text/vnd.fmi.flexstor", ".flx" },
                { "text/vnd.wap.wml", ".wml" },
                { "text/vnd.wap.wmlscript", ".wmls" },
                { "text/webviewhtml", ".htt" },
                { "text/x-asm", ".asm" },
                { "text/x-audiosoft-intra", ".aip" },
                { "text/x-coffeescript", ".coffee" },
                { "text/x-component", ".htc" },
                { "text/x-la-asf", ".lsx" },
                { "text/x-pascal", ".p" },
                { "text/x-script", ".hlb" },
                { "text/x-script.elisp", ".el" },
                { "text/x-script.ksh", ".ksh" },
                { "text/x-script.phyton", ".py" },
                { "text/x-script.rexx", ".rexx" },
                { "text/x-script.sh", ".sh" },
                { "text/x-script.tcl", ".tcl" },
                { "text/x-script.tcsh", ".tcsh" },
                { "text/x-script.zsh", ".zsh" },
                { "text/x-server-parsed-html", ".ssi" },
                { "text/x-setext", ".etx" },
                { "text/x-speech", ".talk" },
                { "text/x-uil", ".uil" },
                { "text/x-uuencode", ".uu" },
                { "text/x-vcalendar", ".vcs" },
                { "text/xml", ".xml" },
                { "video/3gpp", ".3gp" },
                { "video/3gpp2", ".3g2" },
                { "video/animaflex", ".afl" },
                { "video/avi", ".avi" },
                { "video/avs-video", ".avs" },
                { "video/dl", ".dl" },
                { "video/fli", ".fli" },
                { "video/gl", ".gl" },
                { "video/mp4", ".mp4" },
                { "video/mpeg", ".mpg" },
                { "video/ogg", ".ogv" },
                { "video/quicktime", ".mov" },
                { "video/vdo", ".vdo" },
                { "video/vnd.rn-realvideo", ".rv" },
                { "video/vnd.vivo", ".vivo" },
                { "video/vosaic", ".vos" },
                { "video/webm", ".webm" },
                { "video/x-amt-demorun", ".xdr" },
                { "video/x-amt-showrun", ".xsr" },
                { "video/x-atomic3d-feature", ".fmf" },
                { "video/x-dv", ".dif" },
                { "video/x-isvideo", ".isu" },
                { "video/x-ivf", ".ivf" },
                { "video/x-motion-jpeg", ".mjpg" },
                { "video/x-ms-asf", ".asf" },
                { "video/x-ms-wmp", ".wmp" },
                { "video/x-ms-wmv", ".wmv" },
                { "video/x-ms-wmx", ".wmx" },
                { "video/x-ms-wvx", ".wvx" },
                { "video/x-qtc", ".qtc" },
                { "video/x-sgi-movie", ".movie" }
            };
        }
        #endregion

        #region GetMimeType
        /// <summary>
        ///     Get the MIME-type of a file.
        /// </summary>
        /// <remarks>
        ///     Gets the MIME-type of a file based on the file extension.
        /// </remarks>
        /// <returns>The MIME-type.</returns>
        /// <param name="fileName">The file name.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="fileName" /> is <c>null</c>.
        /// </exception>
        public static string GetMimeType(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            var extension = Path.GetExtension(fileName);

            mimeTypes.TryGetValue(extension, out var mimeType);

            return mimeType ?? "application/octet-stream";
        }
        #endregion

        #region TryGetExtension
        /// <summary>
        ///     Get the standard file extension for a MIME-type.
        /// </summary>
        /// <remarks>
        ///     Gets the standard file extension for a MIME-type.
        /// </remarks>
        /// <returns><c>true</c> if the extension is known for the specified MIME-type; otherwise, <c>false</c>.</returns>
        /// <param name="mimeType">The MIME-type.</param>
        /// <param name="extension">The file name extension for the specified MIME-type.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="mimeType" /> is <c>null</c>.
        /// </exception>
        public static bool TryGetExtension(string mimeType, out string extension)
        {
            if (mimeType is null)
                throw new ArgumentNullException(nameof(mimeType));

            return extensions.TryGetValue(mimeType, out extension);
        }
        #endregion

        #region GetExtensionFromMimeType
        /// <summary>
        ///     Returns the file extension for the given <paramref name="mimeType" />. An empty string
        ///     is returned when the mimetype is not found.
        /// </summary>
        /// <param name="mimeType">The mime type</param>
        /// <returns></returns>
        public static string GetExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return string.Empty;

            switch (mimeType.ToLowerInvariant())
            {
                case "application/fractals":
                    return ".fif";

                case "application/futuresplash":
                    return ".spl";

                case "application/hta":
                    return ".hta";

                case "application/mac-binhex40":
                    return ".hqx";

                case "application/ms-infopath.xml":

                case "application/ms-vsi":
                    return ".vsi";

                case "application/msaccess":
                    return ".accdb";

                case "application/msaccess.addin":
                    return ".accda";

                case "application/msaccess.cab":
                    return ".accdc";

                case "application/msaccess.exec":
                    return ".accde";

                case "application/msaccess.ftemplate":
                    return ".accft";

                case "application/msaccess.runtime":
                    return ".accdr";

                case "application/msaccess.template":
                    return ".accdt";

                case "application/msaccess.webapplication":
                    return ".accdw"
                        ;
                case "application/msonenote":
                    return ".one";

                case "application/msword":
                    return ".doc";

                case "application/opensearchdescription+xml":
                    return ".osdx";

                case "application/oxps":
                    return ".oxps";

                case "application/pdf":
                    return ".pdf";

                case "application/pkcs10":
                    return ".p10";

                case "application/pkcs7-mime":
                    return ".p7c";

                case "application/pkcs7-signature":
                    return ".p7s";

                case "application/pkix-cert":
                    return ".cer";

                case "application/pkix-crl":
                    return ".crl";

                case "application/postscript":
                    return ".ps";

                case "application/tif":
                    return ".tif";

                case "application/tiff":
                    return ".tiff";

                case "application/vnd.adobe.acrobat-security-settings":
                    return ".acrobatsecuritysettings";

                case "application/vnd.adobe.pdfxml":
                    return ".pdfxml";

                case "application/vnd.adobe.pdx":
                    return ".pdx";

                case "application/vnd.adobe.xdp+xml":
                    return ".xdp";

                case "application/vnd.adobe.xfd+xml":
                    return ".xfd";

                case "application/vnd.adobe.xfdf":
                    return ".xfdf";

                case "application/vnd.fdf":
                    return ".fdf";

                case "application/vnd.ms-excel":
                    return ".xls";

                case "application/vnd.ms-excel.12":
                    return ".xlsx";

                case "application/vnd.ms-excel.addin.macroEnabled.12":
                    return ".xlam";

                case "application/vnd.ms-excel.sheet.binary.macroEnabled.12":
                    return ".xlsb";

                case "application/vnd.ms-excel.sheet.macroEnabled.12":
                    return ".xlsm";

                case "application/vnd.ms-excel.template.macroEnabled.12":
                    return ".xltm";

                case "application/vnd.ms-officetheme":
                    return ".thmx";

                case "application/vnd.ms-pki.certstore":
                    return ".sst";

                case "application/vnd.ms-pki.pko":
                    return ".pko";

                case "application/vnd.ms-pki.seccat":
                    return ".cat";

                case "application/vnd.ms-powerpoint":
                    return ".ppt";

                case "application/vnd.ms-powerpoint.12":
                    return ".pptx";

                case "application/vnd.ms-powerpoint.addin.macroEnabled.12":
                    return ".ppam";

                case "application/vnd.ms-powerpoint.presentation.macroEnabled.12":
                    return ".pptm";

                case "application/vnd.ms-powerpoint.slide.macroEnabled.12":
                    return ".sldm";

                case "application/vnd.ms-powerpoint.slideshow.macroEnabled.12":
                    return ".ppsm";

                case "application/vnd.ms-powerpoint.template.macroEnabled.12":
                    return ".potm";

                case "application/vnd.ms-publisher":
                    return ".pub";

                case "application/vnd.ms-visio.viewer":
                    return ".vsd";

                case "application/vnd.ms-word.document.12":
                    return ".docx";

                case "application/vnd.ms-word.document.macroEnabled.12":
                    return ".docm";

                case "application/vnd.ms-word.template.12":
                    return ".dotx";

                case "application/vnd.ms-word.template.macroEnabled.12":
                    return ".dotm";

                case "application/vnd.ms-wpl":
                    return ".wpl";

                case "application/vnd.ms-xpsdocument":
                    return ".xps";

                case "application/vnd.oasis.opendocument.presentation":
                    return ".odp";

                case "application/vnd.oasis.opendocument.spreadsheet":
                    return ".ods";

                case "application/vnd.oasis.opendocument.text":
                    return ".odt";

                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                    return ".pptx";

                case "application/vnd.openxmlformats-officedocument.presentationml.slide":
                    return ".sldx";

                case "application/vnd.openxmlformats-officedocument.presentationml.slideshow":
                    return ".ppsx";

                case "application/vnd.openxmlformats-officedocument.presentationml.template":
                    return ".potx";

                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return ".xlsx";

                case "application/vnd.openxmlformats-officedocument.spreadsheetml.template":
                    return ".xltx";

                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";

                case "application/vnd.openxmlformats-officedocument.wordprocessingml.template":
                    return ".dotx";

                case "application/windows-appcontent+xml":
                    return ".appcontent-ms";

                case "application/x-compress":
                    return ".z";

                case "application/x-compressed":
                    return ".tgz";

                case "application/x-dtcp1":
                    return ".dtcp-ip";

                case "application/x-gzip":
                    return ".gz";

                case "application/x-jtx+xps":
                    return ".jtx";

                case "application/x-latex":
                    return ".latex";

                case "application/x-mix-transfer":
                    return ".nix";

                case "application/x-mplayer2":
                    return ".asx";

                case "application/x-ms-application":
                    return ".application";

                case "application/x-ms-vsto":
                    return ".vsto";

                case "application/x-ms-wmd":
                    return ".wmd";

                case "application/x-ms-wmz":
                    return ".wmz";

                case "application/x-ms-xbap":
                    return ".xbap";

                case "application/x-mswebsite":
                    return ".website";

                case "application/x-pkcs12":
                    return ".p12";

                case "application/x-pkcs7-certificates":
                    return ".p7b";

                case "application/x-pkcs7-certreqresp":
                    return ".p7r";

                case "application/x-shockwave-flash":
                    return ".swf";

                case "application/x-skype":
                    return ".skype";

                case "application/x-stuffit":
                    return ".sit";

                case "application/x-tar":
                    return ".tar";

                case "application/x-troff-man":
                    return ".man";

                case "application/x-wlpg-detect":
                    return ".wlpginstall";

                case "application/x-wlpg3-detect":
                    return ".wlpginstall3";

                case "application/x-wmplayer":
                    return ".asx";

                case "application/x-x509-ca-cert":
                    return ".cer";

                case "application/x-zip-compressed":
                    return ".zip";

                case "application/xaml+xml":
                    return ".xaml";

                case "application/xhtml+xml":
                    return ".xht";

                case "application/xml":
                    return ".xml";

                case "application/xps":
                    return ".xps";

                case "application/zip":
                    return ".zip";

                case "audio/3gpp":
                    return ".3gp";

                case "audio/3gpp2":
                    return ".3g2";

                case "audio/aiff":
                    return ".aiff";

                case "audio/basic":
                    return ".au";

                case "audio/ec3":
                    return ".ec3";

                case "audio/l16":
                    return ".lpcm";

                case "audio/mid":
                    return ".mid";

                case "audio/midi":
                    return ".mid";

                case "audio/mp3":
                    return ".mp3";

                case "audio/mp4":
                    return ".m4a";

                case "audio/mpeg":
                    return ".mp3";

                case "audio/mpegurl":
                    return ".m3u";

                case "audio/mpg":
                    return ".mp3";

                case "audio/scpls":
                    return ".pls";

                case "audio/vnd.dlna.adts":
                    return ".adts";

                case "audio/vnd.dolby.dd-raw":
                    return ".ac3";

                case "audio/wav":
                    return ".wav";

                case "audio/x-aiff":
                    return ".aiff";

                case "audio/x-mid":
                    return ".mid";

                case "audio/x-midi":
                    return ".mid";

                case "audio/x-mp3":
                    return ".mp3";

                case "audio/x-mpeg":
                    return ".mp3";

                case "audio/x-mpegurl":
                    return ".m3u";

                case "audio/x-mpg":
                    return ".mp3";

                case "audio/x-ms-wax":
                    return ".wax";

                case "audio/x-ms-wma":
                    return ".wma";

                case "audio/x-scpls":
                    return ".pls";

                case "audio/x-wav":
                    return ".wav";

                case "image/bmp":
                    return ".bmp";

                case "image/gif":
                    return ".gif";

                case "image/jpeg":
                    return ".jpg";

                case "image/pjpeg":
                    return ".jpg";

                case "image/png":
                    return ".png";
                case "image/svg+xml":
                    return ".svg";

                case "image/tiff":
                    return ".tiff";

                case "image/vnd.ms-dds":
                    return ".dds";

                case "image/vnd.ms-photo":
                    return ".wdp";

                case "image/x-emf":
                    return ".emf";

                case "image/x-icon":
                    return ".ico";

                case "image/x-png":
                    return ".png";

                case "image/x-wmf":
                    return ".wmf";


                case "interface/x-winamp-lang":
                    return ".wlz";

                case "interface/x-winamp-skin":
                    return ".wsz";

                case "interface/x-winamp3-skin":
                    return ".wal";

                case "message/rfc822":
                    return ".eml";

                case "midi/mid":
                    return ".mid";

                case "model/vnd.dwfx+xps":
                    return ".dwfx";

                case "model/vnd.easmx+xps":
                    return ".easmx";

                case "model/vnd.edrwx+xps":
                    return ".edrwx";

                case "model/vnd.eprtx+xps":
                    return ".eprtx";

                case "pkcs10":
                    return ".p10";

                case "pkcs7-mime":
                    return ".p7m";

                case "pkcs7-signature":
                    return ".p7s";

                case "pkix-cert":
                    return ".cer";

                case "pkix-crl":
                    return ".crl";

                case "text/calendar":
                    return ".ics";

                case "text/css":
                    return ".css";

                case "text/directory":
                    return ".vcf";

                case "text/directory;profile=vCard":
                    return ".vcf";

                case "text/html":
                    return ".htm";

                case "text/json":
                    return ".mcjson";

                case "text/plain":
                    return ".txt";

                case "text/scriptlet":
                    return ".wsc";

                case "text/vcard":
                    return ".vcf";

                case "text/x-component":
                    return ".htc";

                case "text/x-ms-contact":
                    return ".contact";

                case "text/x-ms-iqy":
                    return ".iqy";

                case "text/x-ms-odc":
                    return ".odc";

                case "text/x-ms-rqy":
                    return ".rqy";

                case "text/x-vcard":
                    return ".vcf";

                case "text/xml":
                    return ".xml";

                case "video/3gpp":
                    return ".3gp";

                case "video/3gpp2":
                    return ".3g2";

                case "video/asx":
                    return ".asx";

                case "video/avi":
                    return ".avi";

                case "video/mp4":
                    return ".mp4";

                case "video/mpeg":
                    return ".mpeg";

                case "video/mpg":
                    return ".mpeg";

                case "video/msvideo":
                    return ".avi";

                case "video/quicktime":
                    return ".mov";

                case "video/vnd.dece.mp4":
                    return ".uvu";

                case "video/vnd.dlna.mpeg-tts":
                    return ".tts";

                case "video/wtv":
                    return ".wtv";

                case "video/x-asx":
                    return ".asx";

                case "video/x-mpeg":
                    return ".mpeg";

                case "video/x-mpeg2a":
                    return ".mpeg";

                case "video/x-ms-asf":
                    return ".asx";

                case "video/x-ms-asf-plugin":
                    return ".asx";

                case "video/x-ms-dvr":
                    return ".dvr-ms";

                case "video/x-ms-wm":
                    return ".wm";

                case "video/x-ms-wmv":
                    return ".wmv";

                case "video/x-ms-wmx":
                    return ".wmx";

                case "video/x-ms-wvx":
                    return ".wvx";

                case "video/x-msvideo":
                    return ".avi";

                case "vnd.ms-pki.certstore":
                    return ".sst";

                case "vnd.ms-pki.pko":
                    return ".pko";

                case "vnd.ms-pki.seccat":
                    return ".cat";

                case "x-pkcs12":
                    return ".p12";

                case "x-pkcs7-certificates":
                    return ".p7b";

                case "x-pkcs7-certreqresp":
                    return ".p7r";

                case "x-x509-ca-cert":
                    return ".cer";

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}