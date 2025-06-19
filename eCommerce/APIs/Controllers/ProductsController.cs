using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext context;

        // Create a constructor and inject context for database access
        public ProductsController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                // Fetch products from the database
                var products = await context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                // Fetch a single product by ID
                var product = await context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }

                // Add the new product to the database
                context.Products.Add(product);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("Product ID mismatch");
                }

                // Check if the product exists
                var existingProduct =  context.Products.Any(x=> x.Id == id);
                if (! existingProduct)
                {
                    return NotFound();
                }

                // Update the product details
                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // Find the product to delete
                var product = await context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // Remove the product from the database
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}