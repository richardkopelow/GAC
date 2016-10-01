# GAC (Generic Avatar Creator)
Avatar creation tool for Unity

## How it works
GAC uses bone scaling to modify a base mesh. Once the "DNA" for a character is setup, modifying the character is as easy as setting its "Height", "Foot Size", or any other attribute you setup.

##DNA
The DNA for a character is a collection of Genes. Every Gene has a name, value, and a list of bones to effect with a mask for how they will be influenced by the gene. For instance, a Foot Size gene may include the bones in both feet, each sporting a mask of (1, 1, 1) so that they scale in all directions. This way, when "Foot Size" is set to 2, the feet will become twice as big as those of the base model.

##Base Characters
Base characters are normally a generic model with an Armature and without any fluff pieces, though the Armature is the only necessary part, since the body model can be added as a component. The base character serves as a baseline that will be modified by the character's DNA. A base character needn't be a human or even humanoid. Since the DNA can be setup for any Armature structure, GAC is completely character structure independent letting it be used for human, dogs, monsters, or anything else.

##Character Components
Components are additional things you want to put on the character, such as clothes, replacement features (elf ears, claws, ...), or even body models because all that is really needed for the base character is its Armature. When a component is added to a character, a copy of the template game object is made, added as a child object, and then attached to the character's Armature. This means that the components are standard Unity object and can have colliders, scripts, and any other component. Creating components is exactly like creating any other object in Unity. Any piece of clothing will fit the character (with any modification from the DNA) as long as the component's model is properly skinned to an armature. THe easiest way to do this is to model clothes directly from the base model to make sure it will deform correctly.