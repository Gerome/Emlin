from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsClassifier

def GetKNNClassifier():
	knn_clf = Pipeline((
		("scaler", StandardScaler()),
		("linear_svc", KNeighborsClassifier(n_neighbors=5)),
	))

	return knn_clf
