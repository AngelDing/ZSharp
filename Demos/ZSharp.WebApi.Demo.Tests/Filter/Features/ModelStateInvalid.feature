Feature: ModelStateInvalid
ModelState验证不通过，应该返回Bad Request.

@mytag
Scenario: ModelStateInvalid
	Given ModelState错误信息
	When 执行OnActionExecuting方法
	Then 返回Bad Request
