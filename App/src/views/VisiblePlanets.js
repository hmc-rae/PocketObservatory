import * as React from 'react';
import { Text, View, StyleSheet,TouchableOpacity,Image } from 'react-native';
import Constants from 'expo-constants';

// or any pure javascript modules available in npm

export default function VisiblePlanets() {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Visible Planets</Text>
      <TouchableOpacity style = {styles.button}>
      <View style={styles.itemContainer}>
        <Text style={styles.itemTitle}>Item Title </Text> 
        <Text style={styles.itemDescription}> Item Description </Text>
      </View>
    </TouchableOpacity>

    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#130D45',
    paddingTop: Constants.statusBarHeight,
    flex: 1,
    padding: 8,
  },
  button : {
    borderRadius: 20,
    padding: 10,
    marginBottom: 20,
    display: "block",
  },

  title: {
    fontWeight: 'bold',
    fontSize: 40,
    color: '#FFFFFF',
    margin:20,
  },
  itemContainer: {
    backgroundColor: "#E5E5E5",
    padding:10,
    borderRadius: 5,
  },
  itemTitle: {
    margin: 10,
    fontSize: 30,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'black',
  },
  itemDescription: {
    margin: 10,
    fontSize: 20,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'black',
  },
  
});
