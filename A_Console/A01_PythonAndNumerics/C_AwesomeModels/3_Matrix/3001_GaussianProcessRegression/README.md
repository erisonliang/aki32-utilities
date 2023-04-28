# 💖 Gaussian Process Regression

## 🌟 Abstract

Code set for Gaussian Process Regression.






<br>

## 🌟 Figures

### 🎇 Example

- before optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Example.png" width="666">


- after optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Optimized.png" width="666">


- used kernel
```math
k(\mathbf{x}_i, \mathbf{x}_j) 
=
\sigma_f^2\exp\left(-\frac{1}{2}\left(\frac{\|\mathbf{x}_i - \mathbf{x}_j\|}{\ell}\right)^2\right) 
+
\sigma_l^2\mathbf{x}_i^{\top}\mathbf{x}_j
+ 
\sigma_n^2\delta_{ij}
```


- C# usage
```C#

// build model
var kc1 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
var kse = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
var kc2 = new GaussianProcessRegressionExecuter.ConstantKernel(0.1d);
var kl  = new GaussianProcessRegressionExecuter.LinearKernel(1d);
var kwn = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
var kernel = kc1 * kse + kc2 * kl + kwn;
var gpr = new GaussianProcessRegressionExecuter(kernel);


// (optional) optimize parameters
var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train);
PyPlotWrapper.LinePlot.DrawSimpleGraph(optimizeHistory["LogLikelihood"]);


// fit and predict
(var Y_predict, var Y_cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
var Y_std = Y_cov.Select(x => Math.Sqrt(x)).ToArray();
var Y_95CI = Y_std.ProductForEach(1.96);
var Y_99CI = Y_std.ProductForEach(2.58);

```










<br>

## 🌟 Memo


- ガウス回帰過程について
https://gochikika.ntt.com/Modeling/gp_regression.html


- カーネル一覧
https://observablehq.com/@herbps10/gaussian-processes


- sk-learnの使ってる組み合わせ11個
https://datachemeng.com/kernel_design_in_gpr/


- 尤度（コード内では対数取った）
```math
\mathcal{L}(\mathbf{y}|\mathbf{X},\boldsymbol{\theta})
=
\frac{1}{(2\pi)^{\frac{n}{2}}|\mathbf{Kernel}|^{\frac{1}{2}}}
\exp\left(-\frac{1}{2}\mathbf{y}^{\top}(\mathbf{Kernel})^{-1}\mathbf{y}\right)
```


- flow
<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Algorithm.jpg" width="666">








<br>

## ❗Notice

Any commitment to enhance this repo will be welcomed!








<br>

## ❗Japanese 日本語

ガウス回帰過程のコード集です。

マイナーなカーネルやパラメーター最適化も実装済みです。（部分的に偏微分を未実装なものあり）




<br>
