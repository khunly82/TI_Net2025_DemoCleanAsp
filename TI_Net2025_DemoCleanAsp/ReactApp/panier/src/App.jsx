import { Badge, Button, Dialog, DialogContent, DialogTitle, Table, TableBody, TableCell, TableFooter, TableHead, TableRow } from '@mui/material';
import './App.css'
import { useEffect, useState } from 'react';

function App() {
  const [open, setOpen] = useState(false);
  const [cart, setCart] = useState([]);

  useEffect(() => {
    // la fonction qui sera lancée au chargement du composant
    fetch('https://localhost:7133/api/cart')
      .then(result => result.json())
      .then(data => setCart(data));
  }, []);

  const total = cart.reduce((prev, item)=> prev + item.quantity * item.productPrice / 100, 0)


  function remove(id) {
    fetch('https://localhost:7133/api/cart/' + id, {
      method: 'delete'
    }).then(() => {
      const c = cart.filter(item => item.productId !== id);
      setCart(c);
    })
  }

  return (
    <>
      <div onClick={() => setOpen(true)} >
        <Badge badgeContent={cart.length}>
          Panier
        </Badge>
      </div> 

      <Dialog open={open} onClose={() => setOpen(false)}>
        <DialogTitle>Panier</DialogTitle>
        <DialogContent>
          { cart.length > 0 ? <Table>
            <TableHead>
              <TableRow>
                <TableCell>Nom</TableCell>
                <TableCell>Quantité</TableCell>
                <TableCell>Prix</TableCell>
                <TableCell></TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              { cart.map(item => <TableRow key={item.productId}>
                <TableCell>{item.productName}</TableCell>
                <TableCell>{item.quantity}</TableCell>
                <TableCell>{item.quantity * item.productPrice / 100}€</TableCell>
                <TableCell><Button onClick={() => remove(item.productId)}>&times;</Button></TableCell>
              </TableRow>) }
            </TableBody>
            <TableFooter>
              <TableRow>{total}€</TableRow>
            </TableFooter>
          </Table>
          : <p>Votre panier est vide</p>
          }
        </DialogContent>
      </Dialog>
    </>
  )
}

export default App
