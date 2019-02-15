from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC
from sklearn.preprocessing import PolynomialFeatures

def GetSVMClassifier():
	svm_clf = Pipeline((
        ("poly_features", PolynomialFeatures(degree=3)),
		("scaler", StandardScaler()),
		("linear_svc", SVC(kernel="poly", degree=2, coef0=1, C=5)),
	))

	return svm_clf
