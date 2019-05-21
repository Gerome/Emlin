import os

import numpy as np
import pandas as pd

from generate_inverse_data import remove_outliers_from_feature_list
from data_insights.sort_combinations_by_similarity import sort_by_highest_average
from utils.constants import DEBUG_DATA_PATH
from utils.plot import plot_data


def compare_users():
    user1_data_filepath = "../../../Data/interim/D_KeyboardData_O_1.csv"
    user2_data_filepath = "../../../Data/interim/D_KeyboardData_O_2.csv"
    user3_data_filepath = "../../../Data/interim/D_KeyboardData_O_3.csv"
    user4_data_filepath = "../../../Data/interim/D_KeyboardData_O_4.csv"
    user5_data_filepath = "../../../Data/interim/D_KeyboardData_O_5.csv"
    user6_data_filepath = "../../../Data/interim/D_KeyboardData_O_6.csv"
    user_test_data_filepath = "../../Data/interim/D_KeyboardData_test.csv"

    test1 = "C:/Users/Gerome/AppData/Roaming/Emlin/D_KeyBoardData_O.csv"
    test2 = "C:/Users/Gerome/AppData/Roaming/Emlin/D_KeyBoardData_O_Test.csv"
    test3 = "C:/Users/Gerome/AppData/Roaming/Emlin/D_KeyBoardData_O_Not.csv"
    test4 = "C:/Users/Gerome/AppData/Roaming/Emlin/ValidationNotSet/D_KeyboardData_O_6_Not.csv"

    #user_data_filepaths = [user1_data_filepath, user2_data_filepath, user3_data_filepath, user4_data_filepath, user5_data_filepath, user6_data_filepath]
    user_data_filepaths = [test1, test2, test4, test3]

    data_users = []

    for path in user_data_filepaths:
        data_users.append(pd.read_csv(os.path.join(DEBUG_DATA_PATH, path)))


    unique_IDs_list = []

    for data in data_users:
        unique_IDs = np.unique(data['Id'])
        unique_IDs_list.append(unique_IDs)

    x = set(unique_IDs_list[0])
    y = set(unique_IDs_list[1])

    for shared_comb in x.intersection(y):

        data_of_comb = []

        for data in data_users:
            data_of_comb.append(data.loc[data['Id'] == shared_comb])

        all_x_data = []

        for user in data_of_comb:
            x_data_no_outlier = remove_outliers_from_feature_list(user[['Id', 'HT', 'FT']].values)
            all_x_data.append(x_data_no_outlier)

        plot_data(shared_comb, all_x_data)

compare_users()