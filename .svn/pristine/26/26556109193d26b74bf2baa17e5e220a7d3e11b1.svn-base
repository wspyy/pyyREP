<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Uploadify.aspx.cs" Inherits="Controls_Uploadify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Uploadify</title>
    <link href="Controls/Uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="Controls/Uploadify/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="Controls/Uploadify/swfobject.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#uploadify').uploadify({
                'uploader': 'Controls/Uploadify/uploadify.swf',
                'script': 'Controls/Uploadify/Uploadify.ashx',
                'cancelImg': 'Controls/Uploadify/cancel.png',
                'folder': 'uploads',
                'auto': false, //当文件被添加到队列时，自动上传 
                'multi': true, //设置为true将允许多文件上传 
                'fileExt': '*.jpg;*.gif;*.png', //允许上传的文件后缀 
                'fileDesc': 'Web Image Files (.JPG, .GIF, .PNG)', //在浏览窗口底部的文件类型下拉菜单中显示的文本 
                'sizeLimit': 10485760, //上传文件的大小限制，单位为字节 100k 
                'buttonText': '',
                //'buttonImg': 'Controls/Uploadify/browse.jpg',
                'onAllComplete': function (event, data) { //当上传队列中的所有文件都完成上传时触发   
                    showMessageAndReturn(data.filesUploaded + ' 个文件上传成功!');
                }
            });
        });
        function SaveUpload() {
            var Folder = $("#Folder").val();
            var Tag = $("#Tag").val();

            $("#uploadify").uploadifySettings("scriptData", { 'FolderName': Folder, 'Tag': Tag });
            $('#uploadify').uploadifyUpload();
        }
    </script>
</head>
<body>
    <div class="row">
        <div class="col-md-12">
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-edit"></i>添加信息_多文件</div>
                    <div class="tools">
                        <a class="collapse" href="javascript:;"></a><a class="reload" onclick="CleanQuery();"
                            href="javascript:;"></a><a class="remove" href="javascript:;"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <!-- BEGIN FORM-->
                    <form class="form-horizontal form-bordered form-row-stripped">
                    <div class="form-actions top ">
                        <button class="btn blue" onclick="SaveUpload();" type="button">
                            保 存</button>&nbsp;
                        <button class="btn default" onclick="jumpReturn();" type="button">
                            返 回</button>&nbsp;</div>
                    <div class="form-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-md-3">
                                        文件夹</label><div class="col-md-9">
                                            <select style="width: 140px;" id="Folder" class="form-control" name="Folder">
                                                <option value="">-----请选择-----</option>
                                                <option value="Folder0001">图片管理</option>
                                                <option value="Folder0002">问题笔记</option>
                                            </select></div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-md-3">
                                        标签</label><div class="col-md-9">
                                            <input id="Tag" class="form-control" name="Tag" type="text" />
                                        </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-md-3">
                                        文件</label><div class="col-md-9">
                                            <span class="btn default btn-file">
                                                <input type="file" name="uploadify" id="uploadify" />
                                        </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                    </div>
                    </form>
                    <!-- END FORM-->
                </div>
            </div>
        </div>
    </div>
</body>
</html>
