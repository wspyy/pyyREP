        /*###########################   定义变量  ###############################*/
        var settingList = {
			check: {
				enable: true
			},
			data: {
				simpleData: {
					enable: true
				}
			},
			callback: {
				onClick: onClick
			}
		};
        var settingModule = {
			check: {
				enable: true
			},
            edit: {
                drag:{
                    isCopy : false,
                    isMove : false
                },
				enable: true,
				showRemoveBtn: false,//需要设置是否显示删除按钮的节点 JSON 数据对象
                showRenameBtn: true //需要设置是否显示编辑名称按钮的节点 JSON 数据对象

			},
			data: {
				simpleData: {
					enable: true
				}
			},
			callback: {
				beforeRename: beforeRename
			}
		};

        //保持 赋权 结果
        function saveRights()
		{
		    if($.fn.zTree.getZTreeObj("treeList").getSelectedNodes() == "")
		    {
		        showMessage('请从列表中选择赋权对象');
		    }
		    else
		    {
		        var argsL = $.fn.zTree.getZTreeObj("treeList").getSelectedNodes();
		        if(argsL[0].pId == null)
		        {
		            showMessage('请从列表中选择子节点');
		        }
		        else
		        {
		            var argsM = $.fn.zTree.getZTreeObj("treeModule").getCheckedNodes();
		            var flag = "";
		            $.each(argsM, function() { flag += this.id + '|'}); 
		            
		            var data = {param: "List",config_Id:args["config_Id"], list: argsL[0].id, module:flag};
		            
                    $.post("../Data/SaveModule.ashx",data,             
                    function(msg) {
                       showMessage(msg);
                    });
		        }		        
		    }
		}
		
		function beforeRename(treeId, treeNode, newName,codes) {
			if(newName.length > 0){
			    
			    var data = {param: "ReName", nodeId: treeNode.id, nodeName:newName};
			    
			    $.post("../Data/ReNameModule.ashx", data,
                    function(msg) {
                       showMessage(msg);
                });
            }
            else
            {
                showMessage("不可以为空！");
                return false;
            }
		}


        //点击 列表树 后重新绑定 模块树结构
        function onClick(event, treeId, treeNode, clickFlag) {
			bindModule("&ID="+treeNode.id);
		}	
        
        
        /*###########################   初始化构建树结构  ###############################*/

        
        function bindList()
        {
            $.ajax({
                type: "POST",
                url: "../Data/GetTreeData.ashx?param=" + args["config_Id"],
                contentType: "application/json;charset=utf-8;",
                cache: false,
                async: false ,//同步方式
                success: function (zNodes) {
            	    $.fn.zTree.init($("#treeList"), settingList, eval(zNodes));
    		
                    var zTree = $.fn.zTree.getZTreeObj("treeList"),
		            type = { "Y":"ps", "N":"ps"};
		            zTree.setting.check.chkboxType = type;

                }
            }); 
        }
        
        function bindModule(whereId)
        {
            $.ajax({
                type: "POST",
                url: "../Data/GetTreeData.ashx?param=module&config_Id=" + args["config_Id"] + whereId,
                contentType: "application/json;charset=utf-8;",
                cache: false,
                async: false ,//同步方式
                success: function(zNodes) {
            	    $.fn.zTree.init($("#treeModule"), settingModule, eval(zNodes));
    		
                    var zTree = $.fn.zTree.getZTreeObj("treeModule"),
		            type = { "Y":"ps", "N":"ps"};
		            zTree.setting.check.chkboxType = type;
                    zTree.setting.edit.renameTitle = "重命名";
                }
            }); 
        }
        
