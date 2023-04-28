# 💖 Gaussian Process Regression

## 🌟 Abstract

Code set for Gaussian Process Regression.






<br>

## 🌟 Figures

### 🎇 Example

before optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Example.png" width="666">

after optimization

<img name="" src="https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Images/A01_C_3001_Optimize.png" width="666">

```C#

// build model
var kc = new ConstantKernel(1d);
var kse = new SquaredExponentialKernel(1d);
var kwn = new WhiteNoiseKernel(1 / 15d);
var kernel = kc * kse + kwn;
var gpr = new GaussianProcessRegressionExecuter(kernel);


// (optional) optimize parameters
var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train);
PyPlotWrapper.LinePlot.DrawSimpleGraph(optimizeHistory[1]);


// fit and predict
(var Y_predict, var Y_cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
var Y_std = Y_cov.Select(x => Math.Sqrt(x)).ToArray();
var Y_95CI = Y_std.ProductForEach(1.96);
var Y_99CI = Y_std.ProductForEach(2.58);

```











<br>

## 🌟 Memo


- 尤度
```math
\mathcal{L}(\mathbf{y}|\mathbf{X},\boldsymbol{\theta})=
\frac{1}
{(2\pi)^{\frac{n}{2}}|\mathbf{Kernel}|^{\frac{1}{2}}}
\exp\left(-\frac{1}{2}\mathbf{y}^{\top}(\mathbf{Kernel})^{-1}\mathbf{y}\right)
```


- ガウス回帰過程について
https://gochikika.ntt.com/Modeling/gp_regression.html


- カーネル一覧
https://observablehq.com/@herbps10/gaussian-processes


- sk-learnの使ってる組み合わせ11個
https://datachemeng.com/kernel_design_in_gpr/








<br>

## ❗Notice

Any commitment to enhance this repo will be welcomed!








<br>

## ❗Japanese 日本語

ガウス回帰過程のコード集です。

マイナーなカーネルやパラメーター最適化も実装済みです。






<br>
