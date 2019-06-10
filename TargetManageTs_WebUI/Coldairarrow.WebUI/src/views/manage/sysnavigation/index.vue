<template>
  <div>
    <d2-container>
    <template slot="header">
      <el-form
        :inline="true"
        :model="form"
        :rules="rules"
        ref="form"
        size="mini"
        style="margin-bottom: -18px;">
        <el-form-item label="关键字：" prop="seachKeyword">
          <el-input
            v-model="form.seachKeyword"
            placeholder="请输入菜单名称"
            style="width: 120px;"/>
        </el-form-item>
        <el-form-item label="状态：" prop="seachIsShow">
          <el-select
            v-model="form.seachIsShow"
            placeholder="状态选择"
            style="width: 100px;">
            <el-option label="请选择" value=""/>
            <el-option label="显示" value="1"/>
            <el-option label="隐藏" value="0"/>
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            @click="handleFormSubmit">
            <d2-icon name="search"/>
            查询
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button
            @click="handleFormReset">
            <d2-icon name="refresh"/>
            重置
          </el-button>
        </el-form-item>
        <el-form-item style="float: right;">
          <el-button-group>
            <el-button type="primary" icon="el-icon-document-add" @click="addRow">新增</el-button>
          </el-button-group>
        </el-form-item>
      </el-form>
    </template>
    <el-container>
      <el-aside width="200px">
        <el-tree :data="treeData" :props="treeProps" @node-click="handleNodeClick" :expand-on-click-node="false"></el-tree>
      </el-aside>
      <el-main>
        <div>
           <d2-crud
              ref="d2Crud"
              :columns="tableColumns"
              :data="tableData"
              :loading="tableloading"
              :pagination="tablepagination"
              @pagination-current-change="tablepaginationCurrentChange"
              add-title="新增菜单"
              :add-template="addTemplate"
              :form-options="formOptions"
              @row-add="handleRowAdd"
              @dialog-open="handleDialogOpen"
              @dialog-cancel="handleDialogCancel">
            </d2-crud>
          </div>
      </el-main>
    </el-container>
  </d2-container>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import { GetMenuTrees, GetMenus } from "@api/sys.navigation";
export default {
  title: "基础表格",
  index: 1,
  data() {
    let $this = this
    return {
      treeSelectNodeId:'', //选中的菜单Id
      treeData: [],
      treeProps: {
        children: "children",
        label: "label"
      },
      tableColumns: [
        {
          title: '菜单名称',
          key: 'navName',
          minWidth: 180
        },
        {
          title: '图标',
          key: 'icon',
          width: 120
        },
        {
          title: '路径',
          key: 'path',
          width: 120
        },
        {
          title: '状态',
          key: 'isShow',
          width: 80,
          formatter:function(row, column, cellValue, index){
            return cellValue==1?"显示":"隐藏"
          }
        },
        {
          title: '创建人',
          key: 'craeteUserName'
        },
        {
          title: '创建时间',
          key: 'createTime',
          formatter:function(row, column, cellValue, index){
            return $this.moment(cellValue).format("YYYY-MM-DD")
          }
        }
      ],
      tableData: [],
      tableloading: false,
      tablepagination: {
        currentPage: 1,// 表格当前页
        pageSize: 10,// 表格每页行数
        total: 0// 总数
      },
      tableWebpagination:{
        page:1,// 表格当前页
        rows:10,// 表格每页行数
        records:0// 总数
      },// 后台分页序列化
      form: {
        seachIsShow: '',
        seachKeyword: ''
      },// 筛选条件
      rules: {
        seachKeyword: [ { required: true, message: ' ', trigger: 'change' } ]
        // seachIsShow: [ { required: true, message: '请选择状态', trigger: 'change' } ]        
      },// 筛选条件验证
      addTemplate:{
        ParentId:{
          title: '上级菜单：',
          value: '',
          component: {
            span: 14,
            offset: 0
          }
        },
        NavName: {
          title: '菜单名称：',
          value: '',
          component: {
            span: 14,
            offset: 0,
            name: 'el-input'
          }
        },
        Path: {
          title: '路径：',
          value: '',
          component: {
            span: 14,
            offset: 0
          }
        },
        Icon: {
          title: '图标：',
          value: '',
          component: {
            span: 12,
            offset: 0
          }
        },
        IconSvg: {
          title: 'Svg图标：',
          value: '',
          component: {
            span: 12,
            offset: 0
          }
        },
        IsShow: {
          title: '状态：',
          value: '',
          component: {
            span: 12,
            offset: 0,
            name:'el-switch'
          }
        },
        SortNum: {
          title: '排序：',
          value: '',
          component: {
            span: 12,
            offset: 0,
            name:'el-input-number'
          }
        },
        Remarks: {
          title: '备注：',
          value: '',
          component: {
            span: 24,
            offset: 0,
            name: 'el-input',
            type:'textarea',
            rows:'2'
          }
        }
      },
      formOptions: {
        width:'550px',
        labelWidth: '100px',
        labelPosition: 'right',
        saveButtonSize:'small ',
        saveButtonText:'保存',
        saveLoading: false,
        gutter: 20
      },// 控制表单中label的显示
    };
  },
  computed: {
    ...mapState("d2admin/user", ["info"])
  },
  methods: {
    handleNodeClick(data) {
      this.treeSelectNodeId=data.id;
      this.fetchData();
    },
    GetMenuTreesSelf() {
      var $this = this;
      GetMenuTrees()
        .then(res => {
          $this.treeData = res.data;
        })
        .catch(err => {
          console.log("err: ", err);
        });
    },
    tablepaginationCurrentChange (currentPage) {
      this.tablepagination.currentPage = currentPage
      this.tableWebpagination.page = currentPage
      this.fetchData()
    },
    fetchData () {
      let $this=this;
      this.tableloading = true
      let [condition,keyword,pagination,id]=[this.form.seachIsShow,this.form.seachKeyword,this.tableWebpagination,this.treeSelectNodeId]
      GetMenus({condition,keyword, pagination,id}).then(res => {
        $this.tableData = res.data.rows
        $this.tablepagination.total = res.data.records
        $this.tableWebpagination.records = res.data.records
        $this.tableloading = false
      }).catch(err => {
        console.log('err', err)
        this.tableloading = false
      })
    },
    handleFormSubmit () {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.$emit('submit', this.form)
        } else {
          // this.$message.error({ message: '请完善查询条件', showClose: true })
          // this.$notify.error({
          //   title: '错误',
          //   message: '表单校验失败'
          // })
          return false
        }
      })
    },
    handleFormReset () {
      this.$refs.form.resetFields()
    },
    // 普通的新增
    addRow () {
      this.$refs.d2Crud.showDialog({
        mode: 'add'
      })
    },
    handleRowAdd (row, done) {
      this.formOptions.saveLoading = true
      setTimeout(() => {
        console.log(row)
        this.$message({
          message: '保存成功',
          type: 'success'
        });

        // done可以传入一个对象来修改提交的某个字段
        done({
          address: '我是通过done事件传入的数据！'
        })
        this.formOptions.saveLoading = false
      }, 300)
    },
    handleDialogOpen ({ mode }) {
      // this.$message({
      //   message: '打开模态框，模式为：' + mode,
      //   type: 'success'
      // })
    },
    handleDialogCancel (done) {
      // this.$message({
      //   message: '取消保存',
      //   type: 'warning'
      // });
      done()
    }
  },
  created:function () {
    this.GetMenuTreesSelf()
  }
}
</script>

<style>  
  .el-aside {
    /* background-color: #D3DCE6;
    color: #333;
    text-align: center; 
    line-height: 200px; */
    height: 100%;
    min-height: 400px;
    border-right: 1px solid #d0d4da;
  }
  
  .el-main {
    /* background-color: #E9EEF3;
    color: #333;
    text-align: center; 
    line-height: 160px; */
    height: 100%;
    min-height: 400px;
  }

  .el-container {
    border: 1px solid #d0d4da;
  }

</style>