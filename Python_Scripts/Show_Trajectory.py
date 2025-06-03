# import library
import pandas as pd
import matplotlib.pyplot as plt
import japanize_matplotlib

# import PositionLog data
df = pd.read_csv("PositionLog.csv", encoding='shift_jis')
print(df)

# calculate velocity
df['Delta_time'] = df['time'].diff()
df['Delta_Position_x'] = df['Position_x'].diff()
df['Delta_Position_z'] = df['Position_z'].diff()
df['distance'] = (df['Delta_Position_x'].pow(2) + df['Delta_Position_z'].pow(2)).pow(0.5)
df['velocity_kmh'] = df['distance'] / df['Delta_time'] * 3.6
print(df)

# Show scatter plot
plt.figure(figsize=(10, 6))
scatter = plt.scatter(df['Position_x'], df['Position_z'], c=df['velocity_kmh'], cmap='viridis', s=10)
cbar = plt.colorbar(scatter)
cbar.set_label('速度 [km/h]')
plt.xlabel('Position X')
plt.ylabel('Position Z')
plt.title('移動軌跡')
plt.grid(False)
plt.tight_layout()
plt.show()

# import RoadLine data
road_df = pd.read_csv("RoadLine.csv")
print(road_df)

# Show scatter plot
plt.figure(figsize=(10, 6))
scatter = plt.scatter(df['Position_x'], df['Position_z'], c=df['velocity_kmh'], cmap='viridis', s=10, label='位置データ')
for _, row in road_df.iterrows():
    x_coords = [row['Point1_x'], row['Point2_x']]
    z_coords = [row['Point1_z'], row['Point2_z']]  # "z" is actually the y-axis
    plt.plot(x_coords, z_coords, color='black', linewidth=1)
cbar = plt.colorbar(scatter)
cbar.set_label('速度 [km/h]')
plt.xlabel('Position X')
plt.ylabel('Position Z')
plt.title('移動軌跡(道路線あり)')
plt.grid(False)
# plt.xlim(100,300)  # optionally
# plt.ylim(-50, 70)  # optionally
plt.tight_layout()
plt.show()