# SimpleAccessTokenContainer

## 这个玩意的由来：

前阵子我搞了搞微信的开发，突然有一天我发现，网页老报错，看了下日志发现是我弄了2个网站，他们各自获取自己的accesstoken，然后打架。我研究了一下，貌似框架自带的AccessTokenContainer没有实现跨应用缓存（难道是我用法不对……）总之我转念一想，那就写个文件呗，反正调试接口也会用到AccessToken，写了文件就能直接跑来复制了，于是就有了这个玩意。