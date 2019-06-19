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
            <el-input v-model="seachForm.seachKeyword" placeholder="请输入用户名称" style="width: 120px;"/>
          </el-form-item>
          <el-form-item label="状态：" prop="seachIsEnabled">
            <el-select v-model="seachForm.seachIsEnabled" style="width: 100px;">
              <el-option label="请选择" value/>
              <el-option label="启用" value="1"/>
              <el-option label="停用" value="0"/>
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
        <el-main>
          <template>
            <el-table :data="tableData" style="width: 100%">
              <el-table-column prop="UserName" label="用户名" width="180"></el-table-column>
              <el-table-column prop="RealName" label="真实姓名" width="120"></el-table-column>
              <el-table-column prop="Sex" label="性别" width="120" :formatter="table_SexFormatter"></el-table-column>
              <el-table-column prop="Phone" label="电话" width="120"></el-table-column>
              <el-table-column
                prop="IsEnabled"
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
        title="新增用户"
        :show-close="true"
        :visible.sync="dialogFormVisibleAdd"
        width="35%"
        top="5vh"
      >
        <el-form :model="dialogForm" label-width="120px" size="small" ref="dialogForm">
          <el-form-item label="用户名：" prop="UserName">
            <el-input v-model="dialogForm.UserName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="密码：" prop="Password">
            <el-input
              v-model="dialogForm.Password"
              style="width:250px;"
              placeholder="请输入密码"
              show-password
            ></el-input>
          </el-form-item>
          <el-form-item label="真实姓名：" prop="RealName">
            <el-input v-model="dialogForm.RealName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="性别：" prop="Sex">
            <el-radio v-model="dialogForm.Sex" :label="1">男</el-radio>
            <el-radio v-model="dialogForm.Sex" :label="0">女</el-radio>
          </el-form-item>
          <el-form-item label="出生日期：" prop="Birthday">
            <el-date-picker
              v-model="dialogForm.Birthday"
              type="date"
              placeholder="选择日期"
              style="width:250px;"
            ></el-date-picker>
          </el-form-item>
          <el-form-item label="邮件：" prop="Email">
            <el-input v-model="dialogForm.Email" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="电话：" prop="Phone">
            <el-input v-model="dialogForm.Phone" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="状态：" prop="IsEnabled">
            <el-switch v-model="dialogForm.IsEnabled" active-text="启用" inactive-text="停用"></el-switch>
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
        title="编辑用户"
        :show-close="true"
        :visible.sync="dialogFormVisibleUpdate"
        width="35%"
        top="5vh"
      >
        <el-form :model="dialogForm" label-width="120px" size="small" ref="dialogForm">
          <el-form-item label="用户名：" prop="UserName">
            <el-input v-model="dialogForm.UserName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="密码：" prop="Password">
            <el-input
              v-model="dialogForm.Password"
              style="width:250px;"
              placeholder="请输入密码"
              show-password
            ></el-input>
          </el-form-item>
          <el-form-item label="真实姓名：" prop="RealName">
            <el-input v-model="dialogForm.RealName" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="性别：" prop="Sex">
            <el-radio v-model="dialogForm.Sex" :label="1">男</el-radio>
            <el-radio v-model="dialogForm.Sex" :label="0">女</el-radio>
          </el-form-item>
          <el-form-item label="出生日期：" prop="Birthday">
            <el-date-picker
              v-model="dialogForm.Birthday"
              type="date"
              placeholder="选择日期"
              style="width:250px;"
            ></el-date-picker>
          </el-form-item>
          <el-form-item label="邮件：" prop="Email">
            <el-input v-model="dialogForm.Email" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="电话：" prop="Phone">
            <el-input v-model="dialogForm.Phone" style="width:250px;" clearable></el-input>
          </el-form-item>
          <el-form-item label="状态：" prop="IsEnabled">
            <el-switch v-model="dialogForm.IsEnabled" active-text="启用" inactive-text="停用"></el-switch>
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
import { GetUsers, addUser, updateUser, deleteUser } from "@api/sys.user";
import _ from "lodash";

export default {
  data() {
    return {
      seachForm: {
        seachIsEnabled: "",
        seachKeyword: ""
      }, // 筛选条件
      seachRules: {
        seachKeyword: [{ required: true, message: " ", trigger: "change" }]
        // seachIsEnabled: [ { required: true, message: '请选择状态', trigger: 'change' } ]
      }, // 筛选条件验证
      tableData: [],
      tableloading: false,
      tableWebpagination: {
        page: 1, // 表格当前页
        rows: 5, // 表格每页行数
        records: 0, // 总数
        SortField: "CreateTime", // 排序字段
        SortType: "Asc" // 排序规则
      }, // 后台分页序列化
      dialogForm: {
        Id: "",
        UserId: "",
        UserName: "",
        Password: "",
        RealName: "",
        Sex: 1,
        Birthday: "",
        Email: "",
        Phone: "",
        IsEnabled: true,
        CreateUserId: "",
        CreateUserName: "",
        CreateTime: ""
      },
      dialogFormVisibleAdd: false,
      dialogFormVisibleUpdate: false
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
      return row.IsEnabled ? "启用" : "停用";
    },
    table_SexFormatter(row, column) {
      return row.Sex == 1 ? "男" : "女";
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
      let [condition, keyword, pagination] = [
        this.seachForm.seachIsEnabled,
        this.seachForm.seachKeyword,
        this.tableWebpagination
      ];
      GetUsers({ condition, keyword, pagination })
        .then(res => {
          debugger;
          $this.tableData = res.Data.rows;
          $this.tableWebpagination.records = res.Data.records;
          $this.tableloading = false;
        })
        .catch(err => {
          console.log("err", err);
          this.tableloading = false;
        });
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
      this.dialogForm = _.cloneDeep(row);
    },
    dialogForm_Save_Update() {
      let theData = this.$refs.dialogForm.model;
      updateMenu({ theData })
        .then(res => {
          if (res.Success) {
            this.tableloading = true;
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
  created: function() {
    this.tableDataQuery();
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
