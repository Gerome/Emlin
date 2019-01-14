
# coding: utf-8

# In[1]:


import os
import pandas as pd
DATA_PATH = "Data/"

def load_group_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "GroupedData.csv")
    return pd.read_csv(csv_path)


# In[2]:


group_data = load_group_data()
group_data.head()


# In[3]:


get_ipython().run_line_magic('matplotlib', 'inline')
import matplotlib.pyplot as plt
group_data.hist(bins=50, figsize=(20,15))
plt.show()


# In[4]:


from sklearn.model_selection import train_test_split
train_set, test_set = train_test_split(group_data, test_size = 0.2, random_state=2)


# In[5]:


print(len(train_set), "train +", len(test_set), "test")


# In[6]:


train_set.info()


# In[9]:


import numpy as np
from sklearn.svm import SVC
from sklearn.model_selection import cross_val_score
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler

all_x = group_data[['Id', 'Data']].values
all_y = group_data[['User']].values
all_y = np.ravel(all_y)

svm_clf = Pipeline((
    ("scaler", StandardScaler()),
    ("linear_svc", SVC(kernel="poly", degree=2, coef0=1, C=5, verbose=True)),
))

svm_scores = cross_val_score(svm_clf, all_x, all_y, cv=5, verbose=True, n_jobs=5)
svm_scores.mean()


# In[8]:


from sklearn.ensemble import RandomForestClassifier
forest_clf = RandomForestClassifier(random_state=32)
forest_scores = cross_val_score(forest_clf, X_train, y_train, cv=20)
forest_scores.mean()


# In[ ]:


from sklearn.externals import joblib
svm_clf.fit(all_x, all_y)

