import numpy as np
import pandas as pd
import os
from scipy import stats

DATA_PATH = os.path.dirname(__file__)


def load_group_data(data_path=DATA_PATH):
    #csv_path = os.path.join(data_path, "../../Data/interim/D_KeyboardData_6.csv")
    csv_path = os.path.join(data_path, "test.csv")
    return pd.read_csv(csv_path)


group_data = load_group_data()


most_common_comb = int(group_data['Id'].mode())
group_data = group_data.loc[group_data['Id'] == most_common_comb]

#all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values

group_data.drop(columns=['Id'])
all_x = group_data[['HT', 'FT']].values



def get_rows_index_of_outliers():
    z_scores = np.abs(stats.zscore(all_x))
    z_score_threshold = 1.5;
    indices_above_threshold = np.where(z_scores > z_score_threshold)[0]
    return list(dict.fromkeys(indices_above_threshold))


rows_with_outliers = get_rows_index_of_outliers()

#print(rows_with_outliers)
#print(group_data.mean())
#print(group_data.std())


def remove_outliers_from_feature_list(list_of_data):

    for index in sorted(rows_with_outliers, reverse=True):
        list_of_data = np.delete(list_of_data, index, axis=0)

    return list_of_data


all_x_no_outliers = remove_outliers_from_feature_list(all_x)



flight_time = group_data['FT']
mean_ft = int(flight_time.mean())
std_ft = int(flight_time.std())

max_ft = mean_ft + std_ft * 4
min_ft = mean_ft - std_ft * 4


hold_time = group_data['HT']
mean_ht = int(hold_time.mean())
std_ht = int(hold_time.std())

max_ht = mean_ht + std_ht * 4
min_ht = mean_ht - std_ht * 4


def distance_between(coord1, coord2):
    distance = np.linalg.norm(coord1 - coord2)
    return distance


#mean_di1 = group_data['Di1'].mean()
#std_di1 = group_data['Di1'].std()
data_dif = 5

print(all_x_no_outliers)

for i in range(min_ht, max_ht, data_dif):
    for j in range(min_ft, max_ft, data_dif):


        too_close = False
        for value in all_x_no_outliers:
            euc_distance = distance_between(value, [i,j])#,65])
            if euc_distance < 10:
                too_close = True
                break;

        if not too_close:
            print(str(i) + ";" + str(j))


