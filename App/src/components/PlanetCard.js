import * as React from 'react';
import { Text, View, StyleSheet,TouchableOpacity,Image } from 'react-native';
import Constants from 'expo-constants';
import PlanetDetails from './PlanetDetails';


let order = ["", "first",  "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth"];

// const detailsNavigation = (id, name) => {
//    navigation.
// };

const PlanetCard = ( {planet, all} ) => {
    return (
      <TouchableOpacity style = {all ? stylesA.button: stylesV.button}>  {/* onPress = {() => detailsNavigation(planet.refID, planet.name)}> */}
        <View style={all ? stylesA.itemContainer: stylesV.itemContainer}>
            <Text style={all ? stylesA.itemTitle: stylesV.itemTitle}> {planet.name} </Text> 
            <Text style={all ? stylesA.itemDescription: stylesV.itemDescription}> {`The ${order[planet.order]} planet of the solar system.`} </Text>
        </View>
      </TouchableOpacity>
    );
};

const stylesA = StyleSheet.create({
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
            backgroundColor: "#2A2557",
            padding:10,
            borderRadius: 5,
        },
        itemTitle: {
            margin: 10,
            fontSize: 30,
            fontWeight: 'light',
            textAlign: 'left',
            color: 'white',
        },
        itemDescription: {
            margin: 10,
            fontSize: 20,
            fontWeight: 'light',
            textAlign: 'left',
            color: 'white',
        },
});

    const stylesV = StyleSheet.create({
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

export default PlanetCard;