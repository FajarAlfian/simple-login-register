import React, { useState } from 'react';
import { Container, Box, TextField, Button, Typography } from '@mui/material';
import axios from "axios";

const RegisterPage = () => {
  const [form, setForm] = useState({ username: '', email: '', password: '' });

  const handleChange = (e) => {
      const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };


  const handleSubmit = (e) => {
    e.preventDefault();
      console.log('Data register:', form);

      axios
      .post('http://localhost:5239/api/Auth/register', form)
    .then ((response) => {
        alert("Berhasil Register");
    })
  };

  return (
    <Container maxWidth="sm">
      <Box sx={{ mt: 4, p: 3, boxShadow: 3, borderRadius: 2 }}>
        <Typography variant="h5" component="h1" gutterBottom>
          Register
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate>
          <TextField
            label="Nama"
            name="username"
            value={form.username}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Email"
            name="email"
            value={form.email}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Password"
            name="password"
            type="password"
            value={form.password}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Confirm Password"
            name="confirmPassword"
            type="password"
            fullWidth
            margin="normal"
          />
          <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }}>
            Daftar
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default RegisterPage;
