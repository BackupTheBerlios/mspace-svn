//
// Vim.Components.VimBuffer.Model.VimBufferStrategy.IVimBufferStrategy.cs :
//      An implementation of a IVimBufferStrategy with a gap buffer.
//
// Author:
//      Néstor Salceda Alonso (wizito@gentelibre.org)
// (C) 2005

using System;
using System.Text;

namespace Vim.Components.VimBuffer {
    public class GapVimBufferStrategy : IVimBufferStrategy {
        private char[] buffer = new char [0];

        int gapBeginOffset = 0;
        int gapEndOffset = 0;

        const int minGapLength = 32;
        const int maxGapLength = 256;

        /**
         * Returns Gap's length.
         */
        int GapLength {
            get {return gapEndOffset - gapBeginOffset;}
        }
        
        /**
         * Returns buffer's length.  
         * Note: The length is total length minus gap's length, because gap must
         * be hidden.  Count all char in buffer.
         */
        public int Length {
            get {return buffer.Length - GapLength;}
        }
        
        /**
         * Sets buffer's content, and reset gap position. Later gap will be
         * placed.
         */
        public void SetContent (string text) {
            if (text == null)
                text = String.Empty;
            buffer = text.ToCharArray ();
            gapBeginOffset = gapEndOffset = 0;
        }

        public char this [int offset] {
            get {
                // Si el offset está mas a la izquierda del comienzo del gap,
                // entonces que devuelva el caracter que marca el offset.
                if (offset < gapBeginOffset)
                    return buffer [offset];
                else
                // Si está mas a la derecha o en el mismo sitio, hay que saltar
                // los caracteres inútiles del gap.
                    return buffer [offset + GapLength];
            }
        }
        
        public string GetText (int offset, int length) {
            int end = offset + length;
            // Si el final está mas a la izquierda que el comienzo del gap, el
            // gap no interfiere, se devuelve esa cadena.
            if (end < gapBeginOffset) {
                return new string (buffer, offset, length);
            }
            // Si el compienzo del gac está mas a la izquierda del offset
            // marcado, entonces se deben saltar los caracteres inservibles del
            // gap.
            if (gapBeginOffset < offset) {
                return new string (buffer, offset + GapLength, length);
            }

            // Si no ocurre ninguna de las anteriores, sea que el final esté mas
            // a la derecha del gap o bien que el principio del gap esté en la
            // misma posición o más a la derecha del offset, entonces.
            StringBuilder buf = new StringBuilder ();
            // Concatenamos el buffer, el comienzo del offset, y como longitud
            // pasamos la dirección de comienzo del gac menos el desplazamiento.
            buf.Append (buffer, offset, gapBeginOffset - offset);
            // Concatenamos otra vez el buffer, empezando desde el final de gap
            // hasta el final del buffer, menos la dirección inicial del gap,
            // para no salirnos del buffer.
            buf.Append (buffer, gapEndOffset, end - gapBeginOffset);
            /**
             * Se basa nuevamente en saltar los caracteres inservibles del gap.
             */
            return buf.ToString ();
        }

        public void Insert (int offset, string text) {
            // Conociendo un autómata de pila, entendemos este Insertar y el
            // Remover. 
            this.Replace (offset, 0, text);
        }

        public void Remove (int offset, int length) {
            this.Replace (offset, length, String.Empty);
        }

        public void Replace (int offset, int length, string text) {
            if (text == null)
                text = String.Empty;
            // Ubicamos el gap, en la posición offset + longitud y su longitud
            // será el máximo de 0 y la longitud el texto - la longitud que le
            // pasamos.
            PlaceGap (offset + length, Math.Max (0, text.Length - length));
            // Copiamos tantos caracteres como el Math.Min de la expresión,
            // desde la posición 0 de text hasta la posicón offset de buffer. 
            text.CopyTo (0, buffer, offset, Math.Min (text.Length, length));
            
            // Si la longitud del buffer es menor que la que le hemos pasado,
            // entonces:
            if (text.Length < length) 
                // Decrementamos el compienzo del offset en tantas unidades como
                // la longitud que le pasamos menos la longitud del buffer.
                gapBeginOffset -= length - text.Length;
            else 
                // Si no si la longitud del buffer es mayor que la longitud que
                // le pasamos, entonces:
                if (text.Length > length) {
                    // Calculamos el incremento de longitudes.
                    int deltaLength = text.Length - length;
                    // Movemos el comienzo del gap tantas unidades como el
                    // incremento delta.
                    gapBeginOffset += deltaLength;
                    // Copiamos desde text[length] a buffer[offset+length],
                    // tantas unidades como la longitud del buffer, menos la
                    // longitud que le pasamos.
                    text.CopyTo (length, buffer, offset+length, text.Length - length);
                }
        }
        
        /**
         * Colocación del gap en el buffer en la posición offset con la longitud
         * length.
         */ 
        void PlaceGap (int offset, int length) {
            // Guardamos el valor viejo de la longitud del gap.
            int oldLength = GapLength;
            // Creamos una nueva longitud.
            int newLength = maxGapLength + length;
            // Calculamos un nuevo final del gap
            int newGapEndOffset = offset + newLength;
            // Construimos el buffer, sabiendo que la longitud será la longitud
            // del buffer, mas la del gap menos la longitud veja del gap
            char[] newBuffer = new char [buffer.Length + newLength - oldLength];
            
            // Si la longitud vieja del gap es cero, entonces
            if (oldLength == 0) {
                // Copiamos el buffer viejo al nuevo, teniendo en cuenta que
                // empezamos de cero, y que el numero de elementos será offset.
                Array.Copy (buffer, 0, newBuffer, 0, offset);
                // Volvemos a copiar, saltandonos el gap; esto es, copiamos el
                // buffer viejo a newBuffer, empezando desde offset, hasta el
                // nuevo buffer, empezando por el nuevo final del offset y el
                // numero de elementos será la longitud del nuevo buffer menos
                // la nueva dirección final del offset del gap.
                Array.Copy (buffer, offset, newBuffer, newGapEndOffset, newBuffer.Length - newGapEndOffset);
            }
            else
            //Si no si la longitud vieja del gap no es cero, entonces:
                // Si el offset está mas a la izquierda que el comienzo de la
                // dirección del gap, entonces:  
                if (offset < gapBeginOffset) {
                    // Calculamos el incremento de offsets.
                    int delta = gapBeginOffset - offset;
                    // Copiamos el buffer viejo al buffer nuevo, teniendo en
                    // cuenta que empezamos ambos buffers desde cero y que el
                    // numero de elementos será offset (los que no han llegado
                    // al gap)
                    Array.Copy (buffer, 0, newBuffer, 0, offset);
                    // Copiamos nuevamente el buffer viejo al buffer nuevo,
                    // empezando el viejo desde offset, hasta el nuevo final del
                    // gap, con los elementos delta.
                    Array.Copy (buffer, offset, newBuffer, newGapEndOffset, delta);
                    // Finalmente copiamos el buffer viejo al nuevo, teniendo en
                    // cuenta que empezamos por la posicion del final del gap, y
                    // en el otro lado con la nueva posicion del gap (final
                    // ??¡¡¡) + el numero de elementos, y tantos elementos como
                    // longitud del buffer - longitud del gap.
                    Array.Copy (buffer, gapEndOffset, newBuffer, newGapEndOffset + delta, buffer.Length - gapEndOffset);
                    
                }
                // Si no, el offset está mas a la derecha o en la misma posición
                // que el offset de comienzo del gap.
                else {
                    // Calculamos el numero de elementos delta.
                    int delta = offset - gapBeginOffset;
                    // Copiamos el buffer viejo en el buffer nuevo, desde la
                    // posición inicial y el numero de elementos de
                    // gapBeginOffset.
                    Array.Copy (buffer, 0, newBuffer, 0 , gapBeginOffset);
                    // Copiamos las posiciones del gap, esto es copiamos el
                    // buffer viejo en el buffer nuevo, empezando por el final
                    // del gap, y desde el comienzo del gap; respectivamente con
                    // tantos elementos como delta.
                    Array.Copy (buffer, gapEndOffset, newBuffer, gapBeginOffset, delta);
                    // Copiamos el resto del buffer, del viejo al nuevo;
                    // empezando por el fin de gap + delta y el nuevo final del
                    // gap, y tantos elementos como la nueva longitud del buffer
                    // menos el desplazamiento final del gap.
                    Array.Copy (buffer, gapEndOffset + delta, newBuffer, newGapEndOffset, newBuffer.Length - newGapEndOffset);
                }
            // Actualizamos los valores.
            buffer = newBuffer;
            gapBeginOffset = offset;
            gapEndOffset = newGapEndOffset;
        }

    }
}
