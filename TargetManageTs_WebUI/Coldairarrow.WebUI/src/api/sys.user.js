import request from '@/plugin/axios'

/**
 * 获取菜单集合
 */
function GetUsers (data) {
  return request({
    url: '/api/Base_User/GetDataList',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 新增菜单
 */
function addUser (data) {
  return request({
    url: '/api/Base_User/SaveData',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 编辑菜单
 */
function updateUser (data) {
  return request({
    url: '/api/Base_User/SaveData',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 删除菜单
 */
function deleteUser (data) {
  return request({
    url: '/api/Base_User/DeleteData?ids=' + data.ids,
    method: 'get',
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

export { GetUsers, addUser, updateUser, deleteUser }
