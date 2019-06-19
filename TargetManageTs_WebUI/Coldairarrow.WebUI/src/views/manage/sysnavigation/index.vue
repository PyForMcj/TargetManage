<template>
  <div>
    <d2-container>
      <template slot="header">
        <el-form
          :inline="true"
          :model="seachForm"
          :rules="seachRules"
          ref="form"
          size="mini"
          style="margin-bottom: -18px;"
        >
          <el-form-item label="关键字：" prop="seachKeyword">
            <el-input v-model="seachForm.seachKeyword" placeholder="请输入菜单名称" style="width: 120px;"/>
          </el-form-item>
          <el-form-item label="状态：" prop="seachIsShow">
            <el-select v-model="seachForm.seachIsShow" style="width: 100px;">
              <el-option label="请选择" value/>
              <el-option label="显示" value="1"/>
              <el-option label="隐藏" value="0"/>
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="seachFormSubmit">
              <d2-icon name="search"/>查询
            </el-button>
          </el-form-item>
          <el-form-item>
            <el-button @click="seachFormReset">
              <d2-icon name="refresh"/>重置
            </el-button>
          </el-form-item>
          <el-form-item style="float: right;">
            <el-button-group>
              <el-button type="primary" icon="el-icon-document-add" @click="dialogForm_Add">新增</el-button>
            </el-button-group>
          </el-form-item>
        </el-form>
      </template>
      <el-container>
        <el-aside width="200px">
          <el-tree
            :data="treeData"
            :props="treeProps"
            @node-click="treeNodeClick"
            :expand-on-click-node="false"
          ></el-tree>
        </el-aside>
        <el-main>
          <template>
            <el-table :data="tableData" style="width: 100%">
              <el-table-column prop="NavName" label="菜单名称" width="180"></el-table-column>
              <el-table-column prop="Icon" label="图标" width="120"></el-table-column>
              <el-table-column prop="Path" label="路径" width="120"></el-table-column>
              <el-table-column
                prop="IsShow"
                label="状态"
                width="80"
                :formatter="table_StatusFormatter"
              ></el-table-column>
              <el-table-column prop="CreateUserName" label="创建人"></el-table-column>
              <el-table-column prop="CreateTime" label="创建时间" :formatter="table_DateFormatter"></el-table-column>
              <el-table-column fixed="right" label="操作" width="150">
                <template slot-scope="scope">
                  <el-button @click="dialogForm_Update(scope.$index, scope.row)" size="mini">编辑</el-button>
                  <el-button
                    @click="dialogForm_Delete(scope.$index, scope.row)"
                    type="danger"
                    size="mini"
                  >删除</el-button>
                </template>
              </el-table-column>
            </el-table>
          </template>
          <template>
            <el-pagination
              style="width: 100%; text-align: center; margin-top: 20px;"
              @size-change="paginationSizeChange"
              @current-change="paginationCurrentChange"
              :current-page.sync="tableWebpagination.page"
              :page-size="tableWebpagination.rows"
              layout="total, prev, pager, next, jumper"
              :total="tableWebpagination.records"
              :hide-on-single-page="false"
              :background="false"
            ></el-pagination>
          </template>
        </el-main>
      </el-container>
    </d2-container>
    <template>
      <el-dialog
        @closed="dialogFormClosed"
        title="新增菜单"
        :show-close="true"
        :visible.sync="dialogFormVisibleAdd"
        top="10vh"
      >
        <el-form :model="dialogForm" label-width="120px" size="small" ref="dialogForm">
          <el-form-item label="上级菜单：" prop="ParentId">
            <el-tree-select
              v-model="dialogForm.ParentId"
              :styles="elTreeSelectStyle"
              :selectParams="elTreeSelectParams"
              :treeParams="elTreeParams"
              @node-click="_nodeClickFun"
              ref="treeSelect"
            />
          </el-form-item>
          <el-form-item label="菜单名称：" prop="NavName">
            <el-input v-model="dialogForm.NavName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="路径：" prop="Path">
            <el-input v-model="dialogForm.Path" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="图标：" prop="Icon">
            <d2-icon-select v-model="dialogForm.Icon"/>
          </el-form-item>
          <!-- <el-row>
          <el-col :span="10">
          <el-form-item label="图标：" prop="Icon">
            <d2-icon-select v-model="dialogForm.Icon"/>
          </el-form-item>
          </el-col>
          <el-col :span="10">
          <el-form-item label="Svg图标：" prop="IconSvg">
            <el-input v-model="dialogForm.IconSvg"></el-input>
          </el-form-item>
          </el-col>
          </el-row>-->
          <el-row>
            <el-col :span="10">
              <el-form-item label="状态：" prop="IsShow">
                <el-switch v-model="dialogForm.IsShow" active-text="显示" inactive-text="隐藏"></el-switch>
              </el-form-item>
            </el-col>
            <el-col :span="10">
              <el-form-item label="排序：" prop="SortNum">
                <el-input-number
                  v-model="dialogForm.SortNum"
                  controls-position="right"
                  :min="0"
                  :max="100"
                ></el-input-number>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item label="备注：" prop="Remarks">
            <el-input
              v-model="dialogForm.Remarks"
              type="textarea"
              maxlength="300"
              show-word-limit
              :rows="3"
              clearable
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialogForm_Cancel_Add">取 消</el-button>
          <el-button type="primary" @click="dialogForm_Save_Add">保 存</el-button>
        </div>
      </el-dialog>
    </template>
    <template>
      <el-dialog
        @closed="dialogFormClosed"
        title="编辑菜单"
        :show-close="true"
        :visible.sync="dialogFormVisibleUpdate"
        top="10vh"
      >
        <el-form :model="dialogForm" label-width="120px" size="small" ref="dialogForm">
          <el-form-item label="上级菜单：" prop="ParentId">
            <el-tree-select
              v-model="dialogForm.ParentId"
              :styles="elTreeSelectStyle"
              :selectParams="elTreeSelectParams"
              :treeParams="elTreeParams"
              @node-click="_nodeClickFun"
              ref="treeSelect"
            />
          </el-form-item>
          <el-form-item label="菜单名称：" prop="NavName">
            <el-input v-model="dialogForm.NavName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="路径：" prop="Path">
            <el-input v-model="dialogForm.Path" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="图标：" prop="Icon">
            <d2-icon-select v-model="dialogForm.Icon"/>
          </el-form-item>
          <!-- <el-row>
          <el-col :span="10">
          <el-form-item label="图标：" prop="Icon">
            <d2-icon-select v-model="dialogForm.Icon"/>
          </el-form-item>
          </el-col>
          <el-col :span="10">
          <el-form-item label="Svg图标：" prop="IconSvg">
            <el-input v-model="dialogForm.IconSvg"></el-input>
          </el-form-item>
          </el-col>
          </el-row>-->
          <el-row>
            <el-col :span="10">
              <el-form-item label="状态：" prop="IsShow">
                <el-switch v-model="dialogForm.IsShow" active-text="显示" inactive-text="隐藏"></el-switch>
              </el-form-item>
            </el-col>
            <el-col :span="10">
              <el-form-item label="排序：" prop="SortNum">
                <el-input-number
                  v-model="dialogForm.SortNum"
                  controls-position="right"
                  :min="0"
                  :max="100"
                ></el-input-number>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item label="备注：" prop="Remarks">
            <el-input
              v-model="dialogForm.Remarks"
              type="textarea"
              maxlength="300"
              show-word-limit
              :rows="3"
              clearable
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialogForm_Cancel_Update">取 消</el-button>
          <el-button type="primary" @click="dialogForm_Save_Update">保 存</el-button>
        </div>
      </el-dialog>
    </template>
  </div>
</template>
<script>
import { mapState } from "vuex";
import {
  GetMenuTrees,
  GetMenus,
  addMenu,
  updateMenu,
  deleteMenu
} from "@api/sys.navigation";
import _ from "lodash";

export default {
  data() {
    return {
      treeSelectNodeId: "", // 选中的菜单Id
      treeData: [], // 菜单tree数据
      treeProps: {
        children: "children",
        label: "label"
      },
      seachForm: {
        seachIsShow: "",
        seachKeyword: ""
      }, // 筛选条件
      seachRules: {
        seachKeyword: [{ required: true, message: " ", trigger: "change" }]
        // seachIsShow: [ { required: true, message: '请选择状态', trigger: 'change' } ]
      }, // 筛选条件验证
      tableData: [],
      tableloading: false,
      tableWebpagination: {
        page: 1, // 表格当前页
        rows: 5, // 表格每页行数
        records: 0, // 总数
        SortField: "SortNum",// 排序字段
        SortType: "Asc",// 排序规则
      }, // 后台分页序列化
      dialogForm: {
        Id: "",
        ParentId: "",
        NavName: "",
        Path: "",
        Icon: "",
        IconSvg: "",
        IsShow: true,
        SortNum: "",
        Remarks: "",
        CreateUserId: "",
        CreateUserName: "",
        CreateTime: ""
      },
      dialogFormVisibleAdd: false,
      dialogFormVisibleUpdate: false,
      elTreeSelectStyle: {
        width: "250px"
      }, // inputTree宽度
      elTreeSelectParams: {
        multiple: false, // 是否多选
        clearable: true, // 是否启用清空
        placeholder: "请选择" // 提示信息
      }, // inputTree配置信息
      elTreeParams: {
        clickParent: true, // 在有子级的情况下是否点击父级关闭弹出框,false 只能点击子级关闭弹出框
        filterable: false, // 是否显示搜索框
        "check-strictly": true,
        "default-expand-all": false, // 默认是否展开
        "expand-on-click-node": false, // true点击节点名称展开，false点击三角展开
        "render-content": this._renderFun, // 自定义下拉框内容显示样式
        data: [], // 下拉树的数据
        props: {
          children: "children",
          label: "label",
          disabled: "disabled",
          value: "id"
        }
      }
    };
  },
  computed: {
    ...mapState("d2admin/user", ["info"])
  },
  methods: {
    // 日期格式化
    table_DateFormatter(row, column) {
      return this.moment(row.createTime).format("YYYY-MM-DD");
    },
    // 禁用状态格式化
    table_StatusFormatter(row, column) {
      return row.IsShow? "显示" : "隐藏";
    },
    treeNodeClick(data) {
      this.treeSelectNodeId = data.id;
      this.tableDataQuery();
    },
    GetMenuTreesSelf() {
      var $this = this;
      GetMenuTrees()
        .then(res => {
          $this.treeData = res.Data;
        })
        .catch(err => {
          console.log("err: ", err);
        });
    },
    seachFormSubmit() {
      this.$refs.form.validate(valid => {
        if (valid) {
          this.$emit("submit", this.seachForm);
        } else {
          // this.$message.error({ message: '请完善查询条件', showClose: true })
          // this.$notify.error({
          //   title: '错误',
          //   message: '表单校验失败'
          // })
          return false;
        }
      });
    },
    seachFormReset() {
      this.$refs.form.resetFields();
    },
    paginationSizeChange(val) {
      this.tableWebpagination.rows = val;
      this.tableDataQuery();
    },
    paginationCurrentChange(val) {
      this.tableWebpagination.page = val;
      this.tableDataQuery();
    },
    tableDataQuery() {
      let $this = this;
      this.tableloading = true;
      let [condition, keyword, pagination, id] = [
        this.seachForm.seachIsShow,
        this.seachForm.seachKeyword,
        this.tableWebpagination,
        this.treeSelectNodeId
      ];
      GetMenus({ condition, keyword, pagination, id })
        .then(res => {
          $this.tableData = res.Data.rows;
          $this.tableWebpagination.records = res.Data.records;
          $this.tableloading = false;
        })
        .catch(err => {
          console.log("err", err);
          this.tableloading = false;
        });
    },
    // inputTree点击事件
    _nodeClickFun(data, node, vm) {
      //console.log('this _nodeClickFun', this.values, data, node);
    },
    // inputTree自定义下拉展示
    _renderFun(h, { node, data, store }) {
      return (
        <span class="custom-tree-node">
          <span>{node.label}</span>
        </span>
      );
    },
    dialogForm_Add() {
      this.dialogFormVisibleAdd = true;
    },
    dialogForm_Save_Add() {
      let theData = this.$refs.dialogForm.model;
      addMenu({ theData })
        .then(res => {
          if (res.Success) {
            this.tableloading = true;
            this.GetMenuTreesSelf();
            this.tableData = [];
            this.tableWebpagination.records = 0;
            this.tableloading = false;
            this.dialogFormVisibleAdd = false;
            this.$message.success({ message: res.Msg, showClose: true });
          } else {
            this.$message.error({ message: res.Msg, showClose: true });
          }
        })
        .catch(err => {
          console.log("err", err);
        });
    },
    dialogForm_Cancel_Add() {
      this.dialogFormVisibleAdd = false;
    },
    dialogForm_Update(index, row) {
      this.dialogFormVisibleUpdate = true;
      row.ParentId = row.ParentId
        ? row.ParentId
        : "00000-00000-00000-00000-00000";
      this.dialogForm = _.cloneDeep(row);
    },
    dialogForm_Save_Update() {
      let theData = this.$refs.dialogForm.model;
      updateMenu({ theData })
        .then(res => {
          if (res.Success) {
            this.tableloading = true;
            this.GetMenuTreesSelf();
            this.tableData = [];
            this.tableWebpagination.records = 0;
            this.tableloading = false;
            this.dialogFormVisibleUpdate = false;
            this.$message.success({ message: res.Msg, showClose: true });
          } else {
            this.$message.error({ message: res.Msg, showClose: true });
          }
        })
        .catch(err => {
          console.log("err", err);
        });
    },
    dialogForm_Cancel_Update() {
      this.dialogFormVisibleUpdate = false;
    },
    dialogForm_Delete(index, row) {
      this.$confirm("确定删除？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          let ids = '["' + row.Id + '"]';
          deleteMenu({ ids })
            .then(res => {
              if (res.Success) {
                this.tableloading = true;
                this.GetMenuTreesSelf();
                this.tableData = [];
                this.tableWebpagination.records = 0;
                this.tableloading = false;
                this.$message.success({ message: res.Msg, showClose: true });
              } else {
                this.$message.error({ message: res.Msg, showClose: true });
              }
            })
            .catch(err => {
              console.log("err", err);
            });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消！", 
            showClose: true
          });
        });
    },
    dialogFormClosed() {
      this.$refs.dialogForm.resetFields();
      Object.assign(this.$data.dialogForm, this.$options.data().dialogForm);
    }
  },
  watch: {
    treeData: function(newQuestion, oldQuestion) {
      this.elTreeParams.data = newQuestion;
    }
  },
  created: function() {
    this.GetMenuTreesSelf();
  }
};
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
