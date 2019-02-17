from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.ensemble import RandomForestClassifier

def GetRFTClassifier():
	rft_clf = Pipeline((
		("scaler", StandardScaler()),
		("linear_svc", RandomForestClassifier()),
	))

	return RandomForestClassifier()
