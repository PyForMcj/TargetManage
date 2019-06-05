import request from '@/plugin/axios'

function AccountLogin (data) {
  return request({
    url: '/api/Base_Login/LoginSubmit',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}
function GetCurrentUserInfo (data) {
  return request({
    url: '/api/Base_User/GetCurrentUserInfo?token=' + data.token,
    method: 'get',
    headers: {
      'Content-Type': 'application/json'
    }
  })
}
export { AccountLogin, GetCurrentUserInfo }
