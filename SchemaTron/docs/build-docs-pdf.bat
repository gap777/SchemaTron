

  



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
  <head>
      
<script type="text/javascript">window.NREUM||(NREUM={});NREUM.info={"beacon":"beacon-3.newrelic.com","errorBeacon":"jserror.newrelic.com","licenseKey":"9dfe439095","applicationID":"8763","transactionName":"Il9dRhNbCVtVQhgXQgBTVkFOWgpTVUMYF1oORw==","queueTime":5,"applicationTime":2047,"ttGuid":"","agentToken":null,"agent":"js-agent.newrelic.com/nr-292.min.js","extra":""}</script>
<script type="text/javascript">window.NREUM||(NREUM={}),__nr_require=function a(b,c,d){function e(f){if(!c[f]){var g=c[f]={exports:{}};b[f][0].call(g.exports,function(a){var c=b[f][1][a];return e(c?c:a)},g,g.exports,a,b,c,d)}return c[f].exports}for(var f=0;f<d.length;f++)e(d[f]);return e}({"4O2Y62":[function(a,b){function c(a,b){var c=d[a];return c?c.apply(this,b):(e[a]||(e[a]=[]),void e[a].push(b))}var d={},e={};b.exports=c,c.queues=e,c.handlers=d},{}],handle:[function(a,b){b.exports=a("4O2Y62")},{}],"SvQ0B+":[function(a,b){function c(a){if(a===window)return 0;if(e.call(a,"__nr"))return a.__nr;try{return Object.defineProperty(a,"__nr",{value:d,writable:!1,configurable:!1,enumerable:!1}),d}catch(b){return a.__nr=d,d}finally{d+=1}}var d=1,e=Object.prototype.hasOwnProperty;b.exports=c},{}],id:[function(a,b){b.exports=a("SvQ0B+")},{}],YLUGVp:[function(a,b){function c(){var a=m.info=NREUM.info,b=m.proto="https"===l.split(":")[0]||a.sslForHttp?"https://":"http://";if(a&&a.agent&&a.licenseKey&&a.applicationID){f("mark",["onload",e()]);var c=h.createElement("script");c.src=b+a.agent,h.body.appendChild(c)}}function d(){"complete"===h.readyState&&f("mark",["domContent",e()])}function e(){return(new Date).getTime()}var f=a("handle"),g=window,h=g.document,i="readystatechange",j="addEventListener",k="attachEvent",l=(""+location).split("?")[0],m=b.exports={offset:e(),origin:l};h[j]?(h[j](i,d,!1),g[j]("load",c,!1)):(h[k]("on"+i,d),g[k]("onload",c)),f("mark",["firstbyte",e()])},{handle:"4O2Y62"}],loader:[function(a,b){b.exports=a("YLUGVp")},{}]},{},["YLUGVp"]);</script>
    <title>docs/build-docs-pdf.bat | Git | Assembla</title>
    <link href="https://www.assembla.com/assets/favicon-67a62ab4cfa7a52140bb0c9ad71ce7cb.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://www.assembla.com/assets/favicon-67a62ab4cfa7a52140bb0c9ad71ce7cb.ico" rel="icon" type="image/x-icon" />
    <link href="https://www.assembla.com/assets/favicon-154a7619f119dfbc2cd65ce3151740ae.gif" rel="icon" type="image/gif" />

    <!--[if IE 7]>
      <link href="https://www.assembla.com/assets/ie/ie7-0cffa215c2f9e503f4c603a6c59ca832.css" media="all" rel="stylesheet" type="text/css" />
    <![endif]-->

    <!--[if lte IE 8]>
      <script src="https://www.assembla.com/assets/excanvas.mod-bdcb98c2fa2ca644eedc705dac9a7504.js" type="text/javascript"></script>
      <link href="https://www.assembla.com/assets/ie/ie8-29169f989d540ba06147be5068413f2f.css" media="all" rel="stylesheet" type="text/css" />
    <![endif]-->

    <!--[if lte IE 9]>
      <link href="https://www.assembla.com/assets/ie/ie9-6cf127091462f3d8572dcb0c3e91d1d2.css" media="all" rel="stylesheet" type="text/css" />
    <![endif]-->

    <meta content="authenticity_token" name="csrf-param" />
<meta content="Q7CPnMTr631cwMfWIBXmzmehZx0encpI1hLqZgEFBXs=" name="csrf-token" />
    <link href="https://www.assembla.com/assets/themes/base_app_and_alerts-5cd7a0abc5f962be2a56dc2590c5b85f.css" media="all" rel="stylesheet" type="text/css" />
    <link href="https://www.assembla.com/assets/sections/repos/code_browser-09d0d1c9ab3d06742e023e9da0f37fc1.css" media="all" rel="stylesheet" type="text/css" />
    
    
    <link href="https://www.assembla.com/assets/themes/print-f2b5179ec1d4dbecb4d02e3a7af64bdc.css" media="print" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
      if (!Breakout) { var Breakout = {}; }
          Breakout.space_wiki_name = "xrouter";
          Breakout.space_id = "dk80SM8qCr34-ueJe5cbLA";
        Breakout.space_new_record = false;
        Breakout.controller_name = "spaces/nodes"
        Breakout.action_name = "show"
      Breakout.notifications_enabled = 'true';
        Breakout.enableTrackers = true;
    </script>
    
    
    <script src="https://www.assembla.com/assets/packages/code-78dbcbad9242c4e3eab8b1df7a7229d9.js" type="text/javascript"></script>
      

      
  
  


    <!--[if lte IE 9]>
      <script src="https://www.assembla.com/assets/packages/lte_ie9-4a1a2a81873945fa6aeccea2355dc78d.js" type="text/javascript"></script>
    <![endif]-->

    <!-- prevents swf file caching -->
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    
  </head>

  <body class="locale_en " data-locale="en">
    <div class="outer ">
      
      <!--[if IE 6]>
        <div class="browser-ie6" style="display: none;"><div>
      <![endif]-->
      <div class="b-wrapper ">
        <a name="pagetop"></a>
          <script type="text/javascript">
    var _gaq = _gaq || [];
    _gaq.push(['_setAccount', 'UA-2641193-1']);
    _gaq.push(['_setDomainName', 'assembla.com']);
    _gaq.push(['_setCustomVar', 1, 'Logged', 'false', 1]);
    
    _gaq.push(['_trackPageview']);

    (function() {
      var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
      ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
      (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(ga);
    })();
    
  </script>



        <div class="hidden">
          <a href="#content">Skip to contents</a>
        </div>

        
          

<div id="header-w">
  <div id="header" class="_">
    <div id="header-links">
        <div id="user-box">
                <a href="/features/compare" class="try-assembla">Try Assembla</a>

  <a href="/login" class="login">Log in</a>


        </div>
      <div class=""><span id="space-role">Free/Public Project</span></div>
    </div>
    <div id="logo">
      <div >
        <h1 class="header-w clear-float float-left">
            <a href="/"><img alt="Go to Assembla" src="https://www.assembla.com/assets/assembla-logo-small-2ca27b9f36b7021ff8a7c54677b95ae4.png" /></a>

            <span>XRouter</span>
        </h1>
        
      </div>
    </div>

    <div class="cut">&nbsp;</div>

  </div><!-- /header -->
</div><!-- /header-w -->

        
      <div id="main-menu-w">
        <ul class="clear-float" id="main-menu"><li class="tab"><a href="/spaces/xrouter/team" class="icon-team"><span>Team</span></a></li><li class="tab"><a href="/spaces/xrouter/stream" class="icon-stream"><span>Stream</span></a></li><li class="tab"><a href="/spaces/xrouter/documents" class="icon-documents"><span>Files</span></a></li><li class="tab current fixed"><a href="/code/xrouter/git/nodes" class="icon-source-git"><span>Git</span></a></li><li class="tab"><a href="/spaces/xrouter/scrum" class="icon-scrum"><span>StandUp</span></a></li><li class="tab"><a href="/code/xrouter/subversion/nodes" class="icon-source-svn"><span>SVN</span></a></li><li class="tab"><a href="/spaces/xrouter/wiki" class="icon-wiki"><span>Wiki</span></a></li><li class="tab"><a href="/spaces/xrouter/tickets" class="icon-ticket"><span>Tickets</span></a></li><li class="tab"><a href="/spaces/xrouter/messages" class="icon-messages"><span>Messages</span></a></li><li id="more-tab" style="display:none;"><a href="#" id="main-more-menu">more</a><div class="more_tabs_menu" id="more-menu" style="display: none;"><div class="more_tabs_menu_item"></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/team" class="icon-team"><span>Team</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/stream" class="icon-stream"><span>Stream</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/documents" class="icon-documents"><span>Files</span></a></div><div class="more_tabs_menu_item"><a href="/code/xrouter/git/nodes" class="icon-source-git"><span>Git</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/scrum" class="icon-scrum"><span>StandUp</span></a></div><div class="more_tabs_menu_item"><a href="/code/xrouter/subversion/nodes" class="icon-source-svn"><span>SVN</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/wiki" class="icon-wiki"><span>Wiki</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/tickets" class="icon-ticket"><span>Tickets</span></a></div><div class="more_tabs_menu_item"><a href="/spaces/xrouter/messages" class="icon-messages"><span>Messages</span></a></div></div></li><li class="search-field"><div class="s-hint float-right mr-5 mt-2">
    <span class="s-icon s-icon-info"></span>
    <div class="s-hint-container s-hint-container-left s-large"><div class="s-arrow"></div>
      <strong>Available Commands:</strong>
      <br />
      <em>#number</em> to access a ticket
      <br />
      <em>@user</em> to access user reports


      <br />
      Invite a user by writing his email address or login

      <br />
      You can use quotes &quot; &quot; to search for the exact words
    </div>
</div>


<form accept-charset="UTF-8" action="/spaces/xrouter/search" autocomplete="off" id="search-form" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
  <input id="object_scope_top_right" name="object_scope" type="hidden" value="10" />
  
  <input class="main-search mentionable" data-autocomplete-url="/spaces/xrouter/user_completions_list" data-value="Search" id="q" maxlength="141" name="q" size="15" type="text" value="Search" />
</form></li></ul>
        <script type="text/javascript">
//<![CDATA[
MainMenuTabs.render();
//]]>
</script>
      </div><!-- /main-menu-w -->

      <ul class='menu-submenu'><li ><a href="/code/xrouter/git/nodes" class="first selected">Source</a></li><li ><a href="/code/xrouter/git/commits" class="">Commits</a></li><li ><a href="/code/xrouter/git/compare" class="">Compare</a></li><li ><a href="/code/xrouter/git/merge_requests" class="">Merge Requests</a></li><li ><a href="/code/xrouter/git/forks" class="">Fork Network</a></li><li ><a href="/code/xrouter/git/repo/instructions" class=" last">Instructions</a></li></ul><div class='cut'></div>

        

        <div id="content" class="data-pjax-container">
          
          
          
          

            <div class="repo-header s-toolbar no-margin clear-float">
  <div class="float-right">
  
  <a href="/code/xrouter/git/compare" class="gray-btn"><span class="s-icon s-icon-compare"></span> Compare</a>
  <a href="/code/xrouter/git/forks/new" class="gray-btn"><span class="s-icon s-icon-fork"></span> Fork</a>
</div>


<h1 class="repo-type">xrouter</h1>

<div class="clone-urls-container float-left">

  <div class="clone-urls s-group-btn-small" data-toggle="buttons-radio">
    <a href="git@git.assembla.com:xrouter.git" class="git gray-btn ">GIT</a>
    <a href="https://git.assembla.com/xrouter.git" class="git-http gray-btn">HTTP</a>
    <a href="git://git.assembla.com/xrouter.git" class="git-ro gray-btn active">GIT<span class="small-font dimmed">Read-only</span></a>
  </div>

  <div class="form-clone-url s-form no-margin float-left">
    <input type="text" id="clone-url" value="git://git.assembla.com/xrouter.git" class="no-rounded-left s-large" readonly="readonly">
    <div class="clippy inline"><span class="global-clippy" data-clipboard-text="git://git.assembla.com/xrouter.git" data-copied-hint="copied!" data-copy-hint="copy to clipboard" data-tooltip-position="right"></span></div>
  </div>

</div>

</div>


<div class="repo-files l-table-code">
  <div class="s-box">
    
<div class="node-toolbar s-header">
  <div class="branch-dropdown s-dropdown">
    <a href="#" class="dropdown-btn gray-btn float-left " data-toggle="dropdown"><span><span class="dropdown-btn-type ">Branch</span><em class="dropdown-btn-name">master</em></span><span class="s-icon s-icon-arrow-down ml-5"></span></a>
    
<div class="dropdown-menu w-400 ">
  <div class="dropdown-menu-top">
    <button class="s-close" type="button">&#215;</button>
    <input class="s-large" id="branch-filter" name="branch-filter" placeholder="Filter Branches or Tags" type="text" />
  </div>

  <div class="s-tabs-container">
    <ul class="s-tabs s-tabs-border clear-float">
      <li class="active"><a href="#all" class="tab-all" data-toggle="tab"><span class="tab-title">All</span><span class="tab-count">7</span></a></li>
      <li><a href="#branches" class="tab-branches" data-toggle="tab"><span class="tab-title">Branches</span><span class="tab-count">6</span></a></li>
      <li><a href="#tags" class="tab-tags" data-toggle="tab"><span class="tab-title">Tags</span><span class="tab-count">1</span></a></li>
      
    </ul>

    <div class="cut"></div>

    <div class="dropdown-menu-list">
        <ul id="all" class="branches-list hidden active">
            <h4 class="std-subheader">Recent Tags</h4>
            <span>
              <li class="sg"><a href="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Tag" data-name-escaped="1.0" data-name="1.0" data-url="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag">1.0</a></li>
            </span>
            <div class="no-results">No results</div>

          <h4 class="std-subheader">Branches</h4>
          <li class="sg"><a href="/code/xrouter/git/nodes/develop/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="develop" data-name="develop" data-url="/code/xrouter/git/nodes/develop/docs/build-docs-pdf.bat">develop</a></li><li class="sg"><a href="/code/xrouter/git/nodes/fix-57-solution-2/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="fix-57-solution-2" data-name="fix-57-solution-2" data-url="/code/xrouter/git/nodes/fix-57-solution-2/docs/build-docs-pdf.bat">fix-57-solution-2</a></li><li class="sg"><a href="/code/xrouter/git/nodes/master/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="master" data-name="master" data-url="/code/xrouter/git/nodes/master/docs/build-docs-pdf.bat">master</a></li><li class="sg"><a href="/code/xrouter/git/nodes/simplification-1/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="simplification-1" data-name="simplification-1" data-url="/code/xrouter/git/nodes/simplification-1/docs/build-docs-pdf.bat">simplification-1</a></li><li class="sg"><a href="/code/xrouter/git/nodes/updatetoken-multilocks/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="updatetoken-multilocks" data-name="updatetoken-multilocks" data-url="/code/xrouter/git/nodes/updatetoken-multilocks/docs/build-docs-pdf.bat">updatetoken-multilocks</a></li><li class="sg"><a href="/code/xrouter/git/nodes/Version-2.0/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="Version-2.0" data-name="Version-2.0" data-url="/code/xrouter/git/nodes/Version-2.0/docs/build-docs-pdf.bat">Version-2.0</a></li>
          <div class="no-results">No results</div>

          <h4 class="std-subheader">Tags</h4>
          <li class="sg"><a href="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Tag" data-name-escaped="1.0" data-name="1.0" data-url="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag">1.0</a></li>
          <div class="no-results">No results</div>
        </ul>

        <ul id="branches" class="branches-list sg-container hidden ">
          <li class="sg"><a href="/code/xrouter/git/nodes/develop/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="develop" data-name="develop" data-url="/code/xrouter/git/nodes/develop/docs/build-docs-pdf.bat">develop</a></li><li class="sg"><a href="/code/xrouter/git/nodes/fix-57-solution-2/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="fix-57-solution-2" data-name="fix-57-solution-2" data-url="/code/xrouter/git/nodes/fix-57-solution-2/docs/build-docs-pdf.bat">fix-57-solution-2</a></li><li class="sg"><a href="/code/xrouter/git/nodes/master/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="master" data-name="master" data-url="/code/xrouter/git/nodes/master/docs/build-docs-pdf.bat">master</a></li><li class="sg"><a href="/code/xrouter/git/nodes/simplification-1/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="simplification-1" data-name="simplification-1" data-url="/code/xrouter/git/nodes/simplification-1/docs/build-docs-pdf.bat">simplification-1</a></li><li class="sg"><a href="/code/xrouter/git/nodes/updatetoken-multilocks/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="updatetoken-multilocks" data-name="updatetoken-multilocks" data-url="/code/xrouter/git/nodes/updatetoken-multilocks/docs/build-docs-pdf.bat">updatetoken-multilocks</a></li><li class="sg"><a href="/code/xrouter/git/nodes/Version-2.0/docs/build-docs-pdf.bat" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Branch" data-name-escaped="Version-2.0" data-name="Version-2.0" data-url="/code/xrouter/git/nodes/Version-2.0/docs/build-docs-pdf.bat">Version-2.0</a></li>
          <div class="no-results">No results</div>
        </ul>

        <ul id="tags" class="branches-list sg-container hidden ">
          <li class="sg"><a href="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag" class="branch-name-link sg-text pjax" data-dropdown-button-type-title="Tag" data-name-escaped="1.0" data-name="1.0" data-url="/code/xrouter/git/nodes/1.0/docs/build-docs-pdf.bat?type=tag">1.0</a></li>
          <div class="no-results">No results</div>
        </ul>

    </div>
  </div>
</div><!--- /dropdown-menu --->

  </div>

  

  <ul class="links-list mt-5 mr-5 float-right">


        <li class="action-links">
          <a href="/code/xrouter/git/node/blob/docs/build-docs-pdf.bat?raw=1&amp;rev=a572823b1d5ce125ae7da85e6413ca4b639827df">Raw</a>
          <em>|</em>
        </li>

      <li class="action-links"><a href="/code/xrouter/git/commits/master/docs/build-docs-pdf.bat" class="revision-log" rel="nofollow">Previous Versions</a></li>

        <em>|</em>
  <li>
    <a href="/code/xrouter/git/nodes/master/docs/build-docs-pdf.bat?_format=raw" class="download-icon" rel="nofollow">Download</a>
  </li>


  </ul>

  <div class="repo-breadcrumb">
    <a href="/code/xrouter/git/nodes/master" class="breadcrumb-path breadcrumb-separator pjax">/</a><a href="/code/xrouter/git/nodes/master/docs" class="breadcrumb-path pjax">docs</a><div class="breadcrumb-separator">/</div><div class="breadcrumb-path">build-docs-pdf.bat</div>
    
  </div>

  <div class="cut">&nbsp;</div>
</div>


      <div class="repo-files show-file pjax-start-hide">


          <div class="scroll-div">
            <table class="s-table s-table-code">
              <tbody>
                <tr class="ln-number">
                  <td class="line-number">
                    <pre style="word-break:normal;word-wrap:normal;"><a href="#ln1" class="inline" id="ln1">1</a>
<a href="#ln2" class="inline" id="ln2">2</a>
<a href="#ln3" class="inline" id="ln3">3</a>
<a href="#ln4" class="inline" id="ln4">4</a>
<a href="#ln5" class="inline" id="ln5">5</a>
<a href="#ln6" class="inline" id="ln6">6</a>
<a href="#ln7" class="inline" id="ln7">7</a>
<a href="#ln8" class="inline" id="ln8">8</a>
<a href="#ln9" class="inline" id="ln9">9</a>
<a href="#ln10" class="inline" id="ln10">10</a>
<a href="#ln11" class="inline" id="ln11">11</a>
<a href="#ln12" class="inline" id="ln12">12</a>
<a href="#ln13" class="inline" id="ln13">13</a>
<a href="#ln14" class="inline" id="ln14">14</a>
<a href="#ln15" class="inline" id="ln15">15</a>
<a href="#ln16" class="inline" id="ln16">16</a>
<a href="#ln17" class="inline" id="ln17">17</a>
<a href="#ln18" class="inline" id="ln18">18</a>
<a href="#ln19" class="inline" id="ln19">19</a>
<a href="#ln20" class="inline" id="ln20">20</a>
<a href="#ln21" class="inline" id="ln21">21</a>
<a href="#ln22" class="inline" id="ln22">22</a>
<a href="#ln23" class="inline" id="ln23">23</a>
<a href="#ln24" class="inline" id="ln24">24</a>
<a href="#ln25" class="inline" id="ln25">25</a>
<a href="#ln26" class="inline" id="ln26">26</a></pre>
                  </td>
                  <td class="show-file-content">
                    <pre style="word-break:normal !important;word-wrap:normal !important; white-space:pre !important" class="prettyprint lang-bat">rem --- convert LaTeX files to PDF ---
rem assumes installed LaTeX, eg. in TeXLive

cd DaemonNT\latex
pdflatex refman.tex
makeindex refman.idx
pdflatex refman.tex
rem run pdfalatex again to get cross-references right
pdflatex refman.tex
cp refman.pdf ..\DaemonNT-API-docs.pdf

cd ..\..\SchemaTron\latex
pdflatex refman.tex
makeindex refman.idx
pdflatex refman.tex
pdflatex refman.tex
cp refman.pdf ..\SchemaTron-API-docs.pdf

cd ..\..\XRouter\latex
pdflatex refman.tex
makeindex refman.idx
pdflatex refman.tex
pdflatex refman.tex
cp refman.pdf ..\XRouter-API-docs.pdf

cd ..\..</pre>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
      </div>

    <div class="ajax-load-indicator pjax-start-show pjax-end-hide"><img alt="Ajax-loader" height="16" src="https://www.assembla.com/assets/ajax-loader-329cf294d8d48d231cf9e07fd60e3ae6.gif" style="vertical-align: bottom;" /> Loading, please wait...</div>
  </div>
</div>




        </div><!-- /content -->

          
  

          <div class="push-app"></div>
      </div><!-- /b-wrapper -->

        <div class="cut">&nbsp;</div>

        <div id="footer-w">
  

  <div id="footer">
    

    <p>
      <a href="/">Home</a>
      / <a href="http://api-doc.assembla.com/">Developer API</a>
        / <a href="/features">Tour</a>
        / <a href="https://assembla.com/plans">Get a Project</a>
        &nbsp; - &nbsp; Solutions for <a href="/features/bug-tracking">Bug &amp; Issue Tracking</a>, <a href="/features/collaboration">Collaboration Tools</a>, <a href="http://offers.assembla.com/free-subversion-hosting">Free Subversion Hosting</a>, <a href="http://offers.assembla.com/free-git-hosting">Free GIT Hosting</a>

    </p>


    <p id="copyr-contact">
    Xrouter is powered by Assembla Workspaces. <a href="/">Learn More</a>
</p>

  </div><!-- /footer -->
</div><!-- /footer-w -->



      
  



      
      
    </div>

    
  <script src="https://www.assembla.com/assets/packages/pretty_code-ff8cfaeb3623449b84aa883e55c590d1.js" type="text/javascript"></script>

    <script type="text/javascript">
//<![CDATA[
prettyPrint()
//]]>
</script>

      
  </body>
</html>


